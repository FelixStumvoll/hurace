using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.RaceStatService;
using Hurace.Core.Logic.Util;
using Hurace.Core.Timer;
using Hurace.Dal.Domain;
using Hurace.Dal.Domain.Enums;
using Hurace.Dal.Interface;
using Microsoft.Extensions.Configuration;
using RaceState = Hurace.Dal.Domain.Enums.RaceState;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.Core.Logic.ActiveRaceControlService.Service
{
    public class ActiveRaceControlService : IActiveRaceControlService
    {
        public event Action<StartList>? OnSkierStarted;
        public event Action<StartList>? OnSkierFinished;
        public event Action<StartList>? OnSkierCanceled;
        public event Action<StartList>? OnCurrentSkierDisqualified;
        public event Action<StartList>? OnLateDisqualification;
        public event Action<TimeData>? OnSplitTime;
        public event Action<Race>? OnRaceCanceled;
        public event Action<Race>? OnRaceFinished;
        
        private readonly IRaceStatService _statService;
        private readonly IRaceDao _raceDao;
        private readonly IStartListDao _startListDao;
        private readonly IRaceEventDao _raceEventDao;
        private readonly IRaceDataDao _raceDataDao;
        private readonly ISkierEventDao _skierEventDao;
        private readonly ITimeDataDao _timeDataDao;
        private readonly ISensorDao _sensorDao;
        private IRaceClock? _raceClock;
        public int RaceId { get; set; }
        private int _maxSensorNr;
        private readonly int _maxDiffToAverage;
        private readonly IConfiguration _configuration;

        public ActiveRaceControlService(IRaceDao raceDao, IStartListDao startListDao, IRaceEventDao raceEventDao,
            IRaceDataDao raceDataDao, ISkierEventDao skierEventDao, ITimeDataDao timeDataDao, ISensorDao sensorDao,
            IConfiguration configuration, IRaceStatService statService)
        {
            _maxDiffToAverage = Convert.ToInt32(configuration.GetSection("RaceSettings")["MaxDiffToAverage"]);
            _configuration = configuration;
            _statService = statService;
            _raceDao = raceDao;
            _startListDao = startListDao;
            _raceEventDao = raceEventDao;
            _raceDataDao = raceDataDao;
            _skierEventDao = skierEventDao;
            _timeDataDao = timeDataDao;
            _sensorDao = sensorDao;
        }

        public async Task InitializeAsync()
        {
            _raceClock = await RaceClockProvider.Instance.GetRaceClock();
            if (_raceClock != null)
                _raceClock.TimingTriggered +=
                    async (sensorNumber, dateTime) => await OnTimingTriggered(sensorNumber, dateTime);
            _maxSensorNr = await _sensorDao.GetLastSensorNumber(RaceId) ?? -1;
        }

        private async Task OnTimingTriggered(int sensorNumber, DateTime dateTime)
        {
            if (!await ValidateSensorValue(sensorNumber, dateTime)) return;

            var timeData = await AddTimeData(sensorNumber, dateTime);
            if (timeData != null) OnSplitTime?.Invoke(timeData);

            if (sensorNumber == _maxSensorNr) await CurrentSkierFinished();
        }

        private bool IsTimeInBoundAverage(int milliseconds, int average) =>
            Math.Abs(milliseconds - average) < _maxDiffToAverage;

        private enum SensorSeriesResult
        {
            Ok,
            SensorMissing,
            SensorAlreadyHasValue,
            SensorAfterwardsSet
        }

        private SensorSeriesResult ValidSensorSeries(IEnumerable<TimeData> timeDataList, int sensorNumber)
        {
            var sensorMissing = false;
            var tdList = timeDataList.OrderBy(td => td?.Sensor?.SensorNumber).ToList();

            for (var i = 0; i < _maxSensorNr; i++)
            {
                if (i == sensorNumber && tdList.Any(td => td?.Sensor?.SensorNumber == i))
                    return SensorSeriesResult.SensorAlreadyHasValue;

                if (sensorNumber < i && tdList.Any(td => td?.Sensor?.SensorNumber == i))
                    return SensorSeriesResult.SensorAfterwardsSet;

                if (i == sensorNumber - 1 && tdList.All(td => td?.Sensor?.SensorNumber != i))
                    sensorMissing = true;
            }

            return sensorMissing ? SensorSeriesResult.SensorMissing : SensorSeriesResult.Ok;
        }

        private async Task<bool> ValidateSensorValue(int sensorNumber, DateTime dateTime)
        {
            //minmax check
            if (sensorNumber < 0 || sensorNumber > _maxSensorNr) return false;

            var currentSkier = await GetCurrentSkier();

            //no current skier
            if (currentSkier == null) return false;
            var startTime = await _timeDataDao.GetStartTimeForStartList(currentSkier.SkierId, RaceId);

            //no startTime
            if (startTime == null && sensorNumber != 0) return false;
            var average = (await _timeDataDao.GetAverageTimeForSensor(RaceId, sensorNumber)) ??
                          Convert.ToInt32(_configuration
                                              .GetSection("RaceSettings")[$"SensorAssumptions:{sensorNumber}"]);
            var timeDataList =
                (await _timeDataDao
                    .GetTimeDataForStartList(currentSkier.SkierId, currentSkier.RaceId)).ToList();
            
            return ValidSensorSeries(timeDataList, sensorNumber) switch
            {
                SensorSeriesResult.Ok => true,
                SensorSeriesResult.SensorMissing => IsTimeInBoundAverage(
                    (dateTime - (startTime ?? dateTime)).Milliseconds, average),
                SensorSeriesResult.SensorAlreadyHasValue => false,
                SensorSeriesResult.SensorAfterwardsSet => false,
                _ => false
            };
        }

        private async Task<TimeData?> AddTimeData(int sensorNumber, DateTime dateTime)
        {
            using var scope = ScopeBuilder.BuildTransactionScope();

            var currentSkier = await GetCurrentSkier();
            if (currentSkier == null) return null;
            var raceDataId = await InsertRaceData(RaceDataEvent.SkierSplitTime, RaceId);
            var skierEventId = await InsertSkierEvent(currentSkier.SkierId, currentSkier.RaceId, raceDataId.Value);
            var sensor = await _sensorDao.GetSensorForSensorNumber(sensorNumber, RaceId);
            var startTime = await _timeDataDao.GetStartTimeForStartList(currentSkier.SkierId, RaceId);

            await InsertTimeData(new TimeData
            {
                RaceId = RaceId,
                SensorId = sensor.Id,
                SkierId = currentSkier.SkierId,
                SkierEventId = skierEventId.Value,
                Time = (int) (dateTime - (startTime ?? dateTime)).TotalMilliseconds
            });
            var ret = await _timeDataDao.FindByIdAsync(currentSkier.SkierId, RaceId, sensor.Id);
            scope.Complete();
            return ret;
        }

        public async Task<bool> EnableRaceForSkier()
        {
            var startList = await _startListDao.GetNextSkierForRace(RaceId);
            if (startList == null) return false;
            await UpdateStartListState(startList, RaceDataEvent.SkierStarted, StartState.Running);
            OnSkierStarted?.Invoke(startList);
            return true;
        }

        private async Task UpdateStartListState(StartList startList, RaceDataEvent eventType,
            StartState state)
        {
            startList.StartStateId = (int) state;
            await _startListDao.UpdateAsync(startList);
            var raceDataId = await InsertRaceData(eventType, RaceId);
            if (raceDataId.HasValue) await InsertSkierEvent(startList.SkierId, startList.RaceId, raceDataId.Value);
        }

        private async Task<int?> InsertRaceData(RaceDataEvent eventType, int raceId) =>
            await _raceDataDao.InsertGetIdAsync(new RaceData
            {
                EventTypeId = (int) eventType,
                RaceId = raceId,
                EventDateTime = DateTime.Now
            });

        private async Task InsertTimeData(TimeData timeData) => await _timeDataDao.InsertAsync(timeData);

        private async Task<int?> InsertSkierEvent(int skierId, int raceId, int raceDataId) =>
            await _skierEventDao.InsertGetIdAsync(new SkierEvent
            {
                RaceId = raceId,
                SkierId = skierId,
                RaceDataId = raceDataId
            });

        private async Task CurrentSkierFinished()
        {
            var currentSkier = await GetCurrentSkier();
            if (currentSkier == null) return;

            await UpdateStartListState(currentSkier, RaceDataEvent.SkierFinished,
                                       StartState.Finished);
            OnSkierFinished?.Invoke(currentSkier);
        }

        public Task<StartList?> GetCurrentSkier() => _startListDao.GetCurrentSkierForRace(RaceId);

        public async Task<bool> CancelSkier(int skierId)
        {
            var startList = await _startListDao.GetSkierForRace(skierId, RaceId);
            await UpdateStartListState(startList, RaceDataEvent.SkierCanceled, StartState.Canceled);
            OnSkierCanceled?.Invoke(startList);
            return true;
        }

        public async Task<IEnumerable<StartList>?> GetRemainingStartList() =>
            (await _startListDao.GetStartListForRace(RaceId))
            .Where(sl => sl.StartStateId == (int) StartState.Upcoming);

        public async Task<bool> DisqualifyCurrentSkier()
        {
            var current = await _startListDao.GetCurrentSkierForRace(RaceId);
            if (current == null) return false;
            await UpdateStartListState(current, RaceDataEvent.SkierDisqualified, StartState.Disqualified);
            OnCurrentSkierDisqualified?.Invoke(current);
            return true;
        }

        public async Task<bool> DisqualifyFinishedSkier(int skierId)
        {
            var skier = await _startListDao.FindByIdAsync(skierId, RaceId);
            if (skier == null || skier.StartStateId != (int) StartState.Finished) return false;
            await UpdateStartListState(skier, RaceDataEvent.SkierDisqualified, StartState.Disqualified);
            OnLateDisqualification?.Invoke(skier);
            return true;
        }

        public async Task<int?> GetPossiblePositionForCurrentSkier()
        {
            var current = await GetCurrentSkier();
            if (current == null) return null;
            var lastTimeData =
                (await _timeDataDao.GetTimeDataForStartList(current.SkierId, current.RaceId))
                .OrderByDescending(td => td?.Sensor?.SensorNumber)
                .First();

            var diff = await _statService.GetDifferenceToLeader(lastTimeData);
            if (diff == null) return 1;

            var ranking = await _statService.GetFinishedSkierRanking(RaceId);
            return 1 + ranking?.TakeWhile(raceRanking => (raceRanking?.TimeToLeader ?? 0) < diff.Value.TotalMilliseconds)
                              .Count();
        }

        public async Task<bool> CancelRace()
        {
            var race = await _raceDao.FindByIdAsync(RaceId);
            if (race == null) return false;
            race.RaceStateId = (int) RaceState.Canceled;
            await _raceDao.UpdateAsync(race);
            OnRaceCanceled?.Invoke(race);
            return true;
        }
    }
}