using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Core.Interface.Entities;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using RaceState = Hurace.Dal.Domain.Enums.RaceState;

namespace Hurace.Core.Service
{
    public class RaceStatService : IRaceStatService
    {
        private readonly ISensorDao _sensorDao;
        private readonly ITimeDataDao _timeDataDao;
        private readonly IStartListDao _startListDao;
        private readonly IRaceDao _raceDao;

        public RaceStatService(IStartListDao startListDao, ITimeDataDao timeDataDao, ISensorDao sensorDao,
            IRaceDao raceDao)
        {
            _startListDao = startListDao;
            _timeDataDao = timeDataDao;
            _sensorDao = sensorDao;
            _raceDao = raceDao;
        }

        public async Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId)
        {
            var ranking = (await GetFinishedSkierRanking(raceId)).ToList();
            ranking.AddRange((await GetDisqualifiedSkiers(raceId))
                             .Select(sl => new RaceRanking(sl)));
            return ranking;
        }

        public async Task<IEnumerable<RaceRanking>?> GetFinishedSkierRanking(int raceId, int count = 0)
        {
            var maxSensorNr = await _sensorDao.GetLastSensorNumber(raceId);
            var ranking = new List<RaceRanking>();
            if (maxSensorNr == null) return null;
            var position = 0;
            var equalityIncrease = 1;
            foreach (var timeData in (await _timeDataDao.GetRankingForSensor(raceId, maxSensorNr.Value)).Where(
                td => td.StartList != null))
            {
                if (position != 0)
                    if (ranking[position - 1].Time == timeData.Time)
                    {
                        equalityIncrease++;
                        if (position == count) count++;
                    }
                    else
                    {
                        position += equalityIncrease;
                        equalityIncrease = 1;
                    }
                else position++;

                ranking.Add(new RaceRanking(timeData.StartList!, timeData.Time, position,
                                            timeData.Time - ranking.FirstOrDefault()?.Time ?? 0));
            }

            return count == 0 ? ranking : ranking.Take(count);
        }

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<StartList>> GetDisqualifiedSkiers(int raceId) =>
            _startListDao.GetDisqualifiedSkierForRace(raceId);

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<TimeData>> GetTimeDataForStartList(int raceId,
            int skierId) => _timeDataDao.GetTimeDataForStartList(skierId, raceId);

        public async Task<TimeSpan?> GetDifferenceToLeader(TimeData timeData)
        {
            var lastSensorNumber = await _sensorDao.GetLastSensorNumber(timeData.RaceId);
            if (lastSensorNumber == null) return null;
            var leader =
                (await _timeDataDao.GetRankingForSensor(timeData.RaceId, lastSensorNumber.Value, 2))?.ToList();
            if (leader == null || !leader.Any()) return TimeSpan.Zero; //no leader

            var index = 0;
            if (leader[0].SkierId == timeData.SkierId && leader.Count > 1) index = 1;
            var leaderTime =
                await _timeDataDao.FindByIdAsync(leader[index].SkierId, leader[index].RaceId, timeData.SensorId);

            if (leaderTime == null) return null; //no leader time
            return TimeSpan.FromMilliseconds(timeData.Time - leaderTime.Time);
        }

        public async Task<IEnumerable<TimeDifference>> GetTimeDataForSkierWithDifference(int skierId, int raceId)
        {
            var timeDataList = await _timeDataDao.GetTimeDataForStartList(skierId, raceId);
            var retVal = new List<TimeDifference>();
            foreach (var timeData in timeDataList)
            {
                var res = await GetDifferenceToLeader(timeData);
                if (!res.HasValue) continue;
                retVal.Add(new TimeDifference(timeData, res.Value));
            }

            return retVal;
        }

        public async Task<DateTime?> GetStartTimeForSkier(int skierId, int raceId)
        {
            var sensor = await _sensorDao.GetSensorForSensorNumber(0, raceId);
            if (sensor == null) return null;
            return (await _timeDataDao.FindByIdAsync(skierId, raceId, sensor.Id))
                   ?.SkierEvent?.RaceData?.EventDateTime;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<RaceRanking>?> GetWinnersForRace(int raceId)
        {
            var race = await _raceDao.FindByIdAsync(raceId);
            if (race == null || race.RaceStateId != (int) RaceState.Finished) return null;
            return await GetFinishedSkierRanking(raceId, 3);
        }
    }
}