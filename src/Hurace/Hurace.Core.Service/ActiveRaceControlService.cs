using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Core.Service.Util;
using Hurace.Dal.Domain;
using Hurace.Dal.Domain.Enums;
using Hurace.Dal.Interface;
using RaceState = Hurace.Dal.Domain.Enums.RaceState;
using SensorConfig = Hurace.Core.Interface.Configs.SensorConfig;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.Core.Service
{
    public class ActiveRaceControlService : IActiveRaceControlService
    {
        public event Action<StartList>? OnSkierStarted;
        public event Action<StartList>? OnSkierFinished;
        public event Action<StartList>? OnSkierCancelled;
        public event Action<StartList>? OnCurrentSkierDisqualified;
        public event Action<StartList>? OnLateDisqualification;
        public event Action<TimeData>? OnSplitTime;
        public event Action<Race>? OnRaceCancelled;
        public event Action<Race>? OnRaceFinished;

        private readonly IRaceDao _raceDao;
        private readonly IStartListDao _startListDao;
        private readonly IRaceEventDao _raceEventDao;
        private readonly IRaceDataDao _raceDataDao;
        private readonly ISkierEventDao _skierEventDao;
        private readonly ITimeDataDao _timeDataDao;
        private readonly ISensorDao _sensorDao;
        private readonly IRaceClockProvider _raceClockProvider;
        private readonly IActiveRaceService _activeRaceService;
        private int _maxSensorNr;
        private readonly SensorConfig _sensorConfig;

        public int RaceId { get; }

        public ActiveRaceControlService(int raceId, IRaceDao raceDao, IStartListDao startListDao,
            IRaceEventDao raceEventDao, IRaceDataDao raceDataDao, ISkierEventDao skierEventDao,
            ITimeDataDao timeDataDao, ISensorDao sensorDao,
            IRaceClockProvider raceClockProvider, SensorConfig sensorConfig, IActiveRaceService activeRaceService)
        {
            RaceId = raceId;
            _raceClockProvider = raceClockProvider;
            _sensorConfig = sensorConfig;
            _activeRaceService = activeRaceService;
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
            var clock = await _raceClockProvider.GetRaceClock();
            if (clock != null)
                clock.TimingTriggered +=
                    async (sensorNumber, dateTime) => await OnTimingTriggered(sensorNumber, dateTime);
            _maxSensorNr = await _sensorDao.GetLastSensorNumber(RaceId) ?? -1;
        }

        private async Task OnTimingTriggered(int sensorNumber, DateTime dateTime)
        {
            try
            {
                if (!await ValidateSensorValue(sensorNumber, dateTime)) return;

                var timeData = await AddTimeData(sensorNumber, dateTime);
                if (timeData != null) OnSplitTime?.Invoke(timeData);

                if (sensorNumber == _maxSensorNr) await CurrentSkierFinished();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private enum SensorSeriesResult
        {
            NeedsAverageValidation,
            SensorAlreadyHasValue,
            SensorAfterwardsSet
        }

        private SensorSeriesResult ValidSensorSeries(IEnumerable<TimeData> timeDataList, int sensorNumber)
        {
            var tdList = timeDataList.OrderBy(td => td?.Sensor?.SensorNumber).ToList();

            for (var i = 0; i < _maxSensorNr; i++)
            {
                if (i == sensorNumber && tdList.Any(td => td?.Sensor?.SensorNumber == sensorNumber))
                    return SensorSeriesResult.SensorAlreadyHasValue;

                if (sensorNumber < i && tdList.Any(td => td?.Sensor?.SensorNumber == i))
                    return SensorSeriesResult.SensorAfterwardsSet;
            }

            return SensorSeriesResult.NeedsAverageValidation;
        }

        private bool IsTimeInBoundAverage(int milliseconds, int average) =>
            Math.Abs(milliseconds - average) <= _sensorConfig.MaxDiffToAverage;

        private async Task<bool> ValidateSensorValue(int sensorNumber, DateTime dateTime)
        {
            //minmax check
            if (sensorNumber < 0 || sensorNumber > _maxSensorNr) return false;

            var currentSkier = await _activeRaceService.GetCurrentSkier(RaceId);

            //no current skier
            if (currentSkier == null) return false;
            var startTime = await _timeDataDao.GetStartTimeForStartList(currentSkier.SkierId, RaceId);

            //no startTime
            if (startTime == null && sensorNumber != 0) return false;
            var average = await _timeDataDao.GetAverageTimeForSensor(RaceId, sensorNumber) ??
                          _sensorConfig.SensorAssumptions[sensorNumber];
            var timeDataList =
                (await _timeDataDao
                    .GetTimeDataForStartList(currentSkier.SkierId, currentSkier.RaceId)).ToList();

            return ValidSensorSeries(timeDataList, sensorNumber) switch
            {
                SensorSeriesResult.NeedsAverageValidation => IsTimeInBoundAverage(
                    (int) (dateTime - (startTime ?? dateTime)).TotalMilliseconds,
                    average),
                SensorSeriesResult.SensorAlreadyHasValue => false,
                SensorSeriesResult.SensorAfterwardsSet => false,
                _ => false
            };
        }

        private async Task<TimeData?> AddTimeData(int sensorNumber, DateTime dateTime)
        {
            using var scope = ScopeBuilder.BuildTransactionScope();

            var currentSkier = await _activeRaceService.GetCurrentSkier(RaceId);
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
            scope.Complete();
            return await _timeDataDao.FindByIdAsync(currentSkier.SkierId, RaceId, sensor.Id);
        }

        public async Task<bool> EnableRaceForSkier()
        {
            var startList = await _startListDao.GetNextSkierForRace(RaceId);
            if (startList == null) return false;
            using (var scope = ScopeBuilder.BuildTransactionScope())
            {
                await UpdateStartListState(startList, RaceDataEvent.SkierStarted, StartState.Running);
                scope.Complete();
            }

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

        [ExcludeFromCodeCoverage]
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
            var currentSkier = await _activeRaceService.GetCurrentSkier(RaceId);
            if (currentSkier == null) return;

            await UpdateStartListState(currentSkier, RaceDataEvent.SkierFinished,
                                       StartState.Finished);
            OnSkierFinished?.Invoke(currentSkier);
        }


        public async Task<bool> CancelSkier(int skierId)
        {
            var startList = await _startListDao.FindByIdAsync(skierId, RaceId);
            if (startList == null) return false;
            await UpdateStartListState(startList, RaceDataEvent.SkierCanceled, StartState.Canceled);
            OnSkierCancelled?.Invoke(startList);
            return true;
        }

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

        public async Task<bool> CancelRace()
        {
            var (success, race) = await ChangeRaceState(RaceId, RaceState.Cancelled);
            if (!success || race == null) return false;
            OnRaceCancelled?.Invoke(race);
            return true;
        }

        private async Task<(bool success, Race? race)> ChangeRaceState(int raceId, RaceState state)
        {
            using var scope = ScopeBuilder.BuildTransactionScope();
            var race = await _raceDao.FindByIdAsync(raceId);
            if (race == null) return (false, null);
            race.RaceStateId = (int) state;
            await _raceDao.UpdateAsync(race);
            var raceData = new RaceData
            {
                EventTypeId = (int) state,
                RaceId = race.Id,
                EventDateTime = DateTime.Now
            };
            var raceDataId = await _raceDataDao.InsertGetIdAsync(raceData);
            if (!raceDataId.HasValue) return (false, null);
            raceData.Id = raceDataId.Value;
            if (!await _raceEventDao.InsertAsync(new RaceEvent
            {
                RaceDataId = raceData.Id
            })) return (false, null);
            scope.Complete();
            return (true, race);
        }

        public async Task<bool> StartRace() => (await ChangeRaceState(RaceId, RaceState.Running)).success;

        public async Task<bool> EndRace()
        {
            //todo validation
            var (success, race) = await ChangeRaceState(RaceId, RaceState.Finished);
            if (!success || race == null) return false;
            OnRaceFinished?.Invoke(race);
            return true;
        }
    }
}