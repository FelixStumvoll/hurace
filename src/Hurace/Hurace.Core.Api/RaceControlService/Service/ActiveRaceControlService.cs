using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Api.Util;
using Hurace.Core.Timer;
using Hurace.Dal.Domain;
using Hurace.Dal.Domain.Enums;
using Hurace.Dal.Interface;
using Microsoft.Extensions.Configuration;
using static Hurace.Core.Api.Util.ExceptionWrapper;

namespace Hurace.Core.Api.RaceControlService.Service
{
    public class ActiveRaceControlService : IActiveRaceControlService
    {
        public event Action<TimeData> OnTimeData;
        public event Action<StartList> OnSkierStarted;
        public event Action<StartList> OnSkierFinished;
        public event Action<StartList> OnSkierCanceled;
        public event Action<StartList> OnCurrentSkierDisqualified;
        public event Action<StartList> OnLateDisqualification;
        public event Action<TimeData> OnSplitTime;
        public event Action OnRaceCanceled;
        public event Action OnRaceFinished;

        private readonly IRaceDao _raceDao;
        private readonly IStartListDao _startListDao;
        private readonly IRaceEventDao _raceEventDao;
        private readonly IRaceDataDao _raceDataDao;
        private readonly ISkierEventDao _skierEventDao;
        private readonly ITimeDataDao _timeDataDao;
        private readonly ISensorDao _sensorDao;
        private IRaceClock _raceClock;
        public int RaceId { get; set; }
        private int _maxSensorNr;
        private readonly int _maxDiffToAverage;

        public ActiveRaceControlService(IRaceDao raceDao, IStartListDao startListDao, IRaceEventDao raceEventDao,
            IRaceDataDao raceDataDao, ISkierEventDao skierEventDao, ITimeDataDao timeDataDao, ISensorDao sensorDao,
            IConfiguration configuration)
        {
            _maxDiffToAverage = Convert.ToInt32(configuration.GetSection("RaceSettings")["MaxDiffToAverage"]);

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
            _raceClock.TimingTriggered +=
                async (sensorNumber, dateTime) => await OnTimingTriggered(sensorNumber, dateTime);
            _maxSensorNr = await _sensorDao.GetMaxSensorNr(RaceId) ?? -1;
        }

        private async Task OnTimingTriggered(int sensorNumber, DateTime dateTime)
        {
            if (!await ValidateSensorValue(sensorNumber, dateTime)) return;

            var timeData = await AddTimeData(sensorNumber, dateTime);
            if (timeData != null) OnSplitTime?.Invoke(timeData);

            if (sensorNumber == _maxSensorNr) await CurrentSkierFinished();
        }

        private bool IsTimeInBound(int milliseconds, int average) =>
            Math.Abs(milliseconds - average) > _maxDiffToAverage;

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
            var tdList = timeDataList.OrderBy(td => td.Sensor.SensorNumber).ToList();

            for (var i = 0; i < _maxSensorNr; i++)
            {
                if (i == sensorNumber && tdList.Any(td => td.Sensor.SensorNumber == i))
                    return SensorSeriesResult.SensorAlreadyHasValue;

                if (sensorNumber < i && tdList.Any(td => td.Sensor.SensorNumber == i))
                    return SensorSeriesResult.SensorAfterwardsSet;

                if (i == sensorNumber - 1 && tdList.All(td => td.Sensor.SensorNumber != i))
                    sensorMissing = true;
            }

            // foreach (var timeData in timeDataList.OrderBy(td => td.Sensor.SensorNumber))
            // {
            //     if (sensorNumber == timeData.Sensor.SensorNumber)
            //
            //
            //         if (sensorNumber < timeData.Sensor.SensorNumber)
            //
            //             if (currNumber != timeData.Sensor.SensorNumber && currNumber == sensorNumber - 1)
            //                 currNumber++;
            // }

            return sensorMissing ? SensorSeriesResult.SensorMissing : SensorSeriesResult.Ok;
        }

        private async Task<bool> ValidateSensorValue(int sensorNumber, DateTime dateTime)
        {
            if (sensorNumber < 0 || sensorNumber > _maxSensorNr) return false;
            var currentSkier = await GetCurrentSkier();
            if (currentSkier.Failure || currentSkier.Value == null) return false;
            var startTime = (await _timeDataDao.GetStartTimeForStartList(currentSkier.Value.SkierId, RaceId));

            var average = await _timeDataDao.GetAverageTimeForSensor(RaceId, sensorNumber);
            var timeDataList =
                (await _timeDataDao.GetTimeDataForStartList(currentSkier.Value.SkierId, currentSkier.Value.RaceId))
                .ToList();
            var currentSkierSplitTime = dateTime - (startTime ?? dateTime); //todo check if null => sensorNumber == 0

            var validSeries = ValidSensorSeries(timeDataList, sensorNumber);
            var averageCheck = IsTimeInBound(currentSkierSplitTime.Milliseconds, average ?? 0);

            return validSeries switch
            {
                SensorSeriesResult.Ok => true,
                SensorSeriesResult.SensorMissing => averageCheck,
                SensorSeriesResult.SensorAlreadyHasValue => false,
                SensorSeriesResult.SensorAfterwardsSet => false,
                _ => false
            };
        }

        private async Task<TimeData?> AddTimeData(int sensorNumber, DateTime dateTime)
        {
            var currentSkier = await GetCurrentSkier();
            if (currentSkier.Failure) return null;
            var raceDataId = await InsertRaceData(RaceDataEvent.SkierSplitTime, RaceId);
            if (!raceDataId.HasValue) return null;
            var skierEventId =
                await InsertSkierEvent(currentSkier.Value.SkierId, currentSkier.Value.RaceId, raceDataId.Value);
            if (!skierEventId.HasValue) return null;
            var sensor = await _sensorDao.GetSensorForSensorNumber(sensorNumber, RaceId);
            var startTime = await _timeDataDao.GetStartTimeForStartList(currentSkier.Value.SkierId, RaceId);


            var ret = new TimeData
            {
                RaceId = RaceId,
                SensorId = sensor.Id,
                SkierId = currentSkier.Value.SkierId,
                SkierEventId = skierEventId.Value,
                Time = (int) (dateTime - (startTime ?? dateTime)).TotalMilliseconds
            };
            Console.WriteLine($"Time: {ret.Time}");
            await InsertTimeData(ret);
            return await _timeDataDao.FindByIdAsync(currentSkier.Value.SkierId, RaceId, sensor.Id);
        }

        public async Task<Result<bool, Exception>> EnableRaceForSkier() =>
            await Try(async () =>
            {
                var startList = await _startListDao.GetNextSkierForRace(RaceId);
                await UpdateStartListState(startList, RaceDataEvent.SkierStarted, Constants.StartState.Running);
                OnSkierStarted?.Invoke(startList);
                return true;
            });

        private async Task UpdateStartListState(StartList startList, RaceDataEvent eventType,
            Constants.StartState state)
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
            if (currentSkier.Failure) return;

            await UpdateStartListState(currentSkier.Value, RaceDataEvent.SkierFinished,
                                       Constants.StartState.Finished);
        }

        public async Task<Result<StartList, Exception>> GetCurrentSkier() =>
            await Try(() => _startListDao.GetCurrentSkierForRace(RaceId));

        public async Task<Result<bool, Exception>> CancelSkier(int skierId)
        {
            return await Try(async () =>
            {
                var startList = await _startListDao.GetSkierForRace(skierId, RaceId);
                await UpdateStartListState(startList, RaceDataEvent.SkierCanceled, Constants.StartState.Canceled);
                OnSkierCanceled?.Invoke(startList);
                return true;
            });
        }

        public async Task<Result<IEnumerable<StartList>, Exception>> GetRemainingStartList() =>
            await Try(async () => (await _startListDao.GetStartListForRace(RaceId))
                          .Where(sl => sl.StartStateId == (int) Constants.StartState.Upcoming));

        public async Task<Result<TimeSpan?, Exception>> GetDifferenceToLeader(TimeData timeData) =>
            await TryStruct<TimeSpan>(async () =>
            {
                var first = (await _timeDataDao.GetRankingForSensor(timeData.RaceId, timeData.SensorId, 1))
                    .FirstOrDefault();
                var current = (await _timeDataDao.FindByIdAsync(timeData.SkierId, timeData.RaceId, timeData.SensorId));
                if (first == null) return TimeSpan.Zero;
                var leaderTime =
                    await _timeDataDao.GetTimeDataForSensor(first.SkierId, first.RaceId, timeData.SensorId);

                if (leaderTime == null) return null;

                return TimeSpan.FromMilliseconds(leaderTime.Time - current.Time);
            });

        public async Task<Result<bool, Exception>> CancelRace()
        {
            return Result<bool, Exception>.Err(null);
        }

        public async Task<Result<IEnumerable<TimeDifference>, Exception>> GetTimeDataForSkierWithDifference(int skierId,
            int raceId) =>
            await Try(async () =>
            {
                var timeDataList = await _timeDataDao.GetTimeDataForStartList(skierId, raceId);
                var retVal = new List<TimeDifference>();
                foreach (var timeData in timeDataList)
                {
                    var res = await GetDifferenceToLeader(timeData);
                    if (res.Failure || !res.Value.HasValue) continue;
                    retVal.Add(new TimeDifference
                    {
                        TimeData = timeData,
                        DifferenceToLeader = res.Value.Value.Milliseconds
                    });
                }

                return (IEnumerable<TimeDifference>) retVal;
            });
    }
}