using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Api.Util;
using Hurace.Core.Timer;
using Hurace.Dal.Domain;
using Hurace.Dal.Domain.Enums;
using Hurace.Dal.Interface;
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
        private int _maxDiffToAverage;

        public ActiveRaceControlService(IRaceDao raceDao, IStartListDao startListDao, IRaceEventDao raceEventDao,
            IRaceDataDao raceDataDao, ISkierEventDao skierEventDao, ITimeDataDao timeDataDao, ISensorDao sensorDao)
        {
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
            _maxSensorNr = await _sensorDao.GetMaxSensorNr(RaceId);
        }

        private async Task OnTimingTriggered(int sensorNumber, DateTime dateTime)
        {
            if (sensorNumber < 0 || sensorNumber > _maxSensorNr) return;


            await AddTimeData(sensorNumber, dateTime);

            if (sensorNumber == _maxSensorNr)
            {
                await CurrentSkierFinished();
            }

            // Console.WriteLine("Timer succeeds min max check");
            /*var currentSkier = await GetCurrentSkier();
            var sensorTimeData =
                await _timeDataDao.GetTimeDataForSensor(currentSkier.SkierId, currentSkier.RaceId, sensorNumber);

            if (sensorTimeData != null)
            {
            }*/

            //var sensorReadings = await _timeDataDao.GetTimeDataForStartList(ski)
        }

        private async Task<bool> ValidateSensorValue(int sensorNumber, DateTime dateTime)
        {
            //fixme cache current skier
            var currentSkier = await GetCurrentSkier();
            if (currentSkier.Failure) return false;
            var startTime = (await _timeDataDao.GetStartTimeForStartList(currentSkier.Value.SkierId, RaceId));

            var average = await _timeDataDao.GetAverageTimeForSensor(RaceId, sensorNumber);
            var currentSkierSplitTime = dateTime - startTime;

            if (average == -1)
            {
            }
            else
            {
                if (Math.Abs(currentSkierSplitTime.Milliseconds - average) > _maxDiffToAverage)
                {
                }
                else
                {
                    await AddTimeData(sensorNumber, dateTime);
                    return true;
                }
            }

            /*
             * Load Average
             * Check íf out of average
             * if out of average -> previous sensors out of average
             * if true -> correct
             * if not -> not correct
             * if first check with 
             */
            return false;
        }

        private async Task AddTimeData(int sensorNumber, DateTime dateTime)
        {
            var currentSkier = await GetCurrentSkier();
            if(currentSkier.Failure) return;
            var raceDataId = await InsertRaceData(RaceDataEvent.SkierSplitTime, RaceId);
            var skierEventId = await InsertSkierEvent(currentSkier.Value.SkierId, currentSkier.Value.RaceId, raceDataId);
            var sensor = await _sensorDao.GetSensorForSensorNumber(sensorNumber, RaceId);
            var startTime = await _timeDataDao.GetStartTimeForStartList(currentSkier.Value.SkierId, RaceId);

            await InsertTimeData(new TimeData
            {
                RaceId = RaceId,
                SensorId = sensor.Id,
                SkierId = currentSkier.Value.SkierId,
                SkierEventId = skierEventId,
                Time = (dateTime - startTime).Milliseconds
            });
        }

        public async Task<Result<bool,Exception>> EnableRaceForSkier() =>
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
            await InsertSkierEvent(startList.SkierId, startList.RaceId, raceDataId);
        }

        private async Task<int> InsertRaceData(RaceDataEvent eventType, int raceId) =>
            await _raceDataDao.InsertGetIdAsync(new RaceData
            {
                EventTypeId = (int) eventType,
                RaceId = raceId,
                EventDateTime = DateTime.Now
            });

        private async Task InsertTimeData(TimeData timeData) => await _timeDataDao.InsertAsync(timeData);

        private async Task<int> InsertSkierEvent(int skierId, int raceId, int raceDataId) =>
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

        public async Task<Result<bool,Exception>> CancelSkier(int skierId)
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

        public async Task<Result<bool,Exception>> CancelRace()
        {
            return Result<bool,Exception>.Err(null);
        }
    }
}