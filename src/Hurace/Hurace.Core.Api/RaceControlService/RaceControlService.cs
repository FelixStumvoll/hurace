using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Timer;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using SkierEvent = Hurace.Dal.Domain.SkierEvent;

namespace Hurace.Core.Api.RaceControlService
{
    public class RaceControlService : IRaceControlService
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

        public RaceControlService(IRaceDao raceDao, IStartListDao startListDao, IRaceEventDao raceEventDao,
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

        private async Task OnTimingTriggered(int sensorId, DateTime dateTime)
        {
            if (sensorId < 0 || sensorId > _maxSensorNr) return;


            Console.WriteLine("Timer succeeds min max check");
            var currentSkier = await GetCurrentSkier();
            var sensorTimeData =
                await _timeDataDao.GetTimeDataForSensor(currentSkier.SkierId, currentSkier.RaceId, sensorId);

            if (sensorTimeData != null)
            {
            }

            //var sensorReadings = await _timeDataDao.GetTimeDataForStartList(ski)
        }

        private async Task<bool> ValidateSensorValue(int sensorNumber, DateTime dateTime)
        {
            //fixme cache current skier
            var currentSkier = await GetCurrentSkier();
            var startTime = (await _timeDataDao.GetStartTimeForStartList(currentSkier.SkierId, RaceId));

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
                    await InsertTimeData(sensorNumber, dateTime);
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

        private async Task InsertTimeData(int sensorNumber, DateTime dateTime)
        {
        }

        public async Task<bool> EnableRaceForSkier()
        {
            try
            {
                var startList = await _startListDao.GetNextSkierForRace(RaceId);
                await UpdateStartListState(startList, Constants.StartState.Running);
                OnSkierStarted?.Invoke(startList);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task UpdateStartListState(StartList startList, Constants.StartState state)
        {
            startList.StartStateId = (int) state;
            await _startListDao.UpdateAsync(startList);
            var raceData = new RaceData
            {
                EventTypeId = (int) state,
                RaceId = startList.RaceId,
                EventDateTime = DateTime.Now
            };

            raceData.Id = await _raceDataDao.InsertGetIdAsync(raceData);
            await _skierEventDao.InsertAsync(new SkierEvent
            {
                RaceId = startList.RaceId,
                SkierId = startList.SkierId,
                RaceDataId = raceData.Id
            });
        }

        public async Task<StartList> GetCurrentSkier() => await _startListDao.GetCurrentSkierForRace(RaceId);

        public async Task<bool> CancelSkier(int skierId)
        {
            try
            {
                var startList = await _startListDao.GetSkierForRace(skierId, RaceId);
                await UpdateStartListState(startList, Constants.StartState.Canceled);
                OnSkierCanceled?.Invoke(startList);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<StartList>?> GetRemainingStartList()
        {
            try
            {
                return (await _startListDao.GetStartListForRace(RaceId)).Where(
                    sl => sl.StartStateId == (int) Constants.StartState.Upcoming);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TimeSpan?> GetDifferenceToLeader(TimeData timeData)
        {
            try
            {
                var first = (await _timeDataDao.GetRankingForSensor(timeData.RaceId, timeData.SensorId, 1))
                    .FirstOrDefault();
                var current = (await _timeDataDao.FindByIdAsync(timeData.SkierId, timeData.RaceId, timeData.SensorId));
                if (first == null) return TimeSpan.Zero;
                var leaderTime =
                    await _timeDataDao.GetTimeDataForSensor(first.SkierId, first.RaceId, timeData.SensorId);

                if (leaderTime == null) return null;

                return TimeSpan.FromMilliseconds(leaderTime.Time - current.Time);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CancelRace()
        {
            return false;
        }
    }
}