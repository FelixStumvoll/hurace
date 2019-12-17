using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Api.Models;
using Hurace.Core.Api.Util;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Api.RaceService
{
    internal class RaceService : IRaceService
    {
        private readonly IRaceDao _raceDao;
        private readonly IStartListDao _startListDao;
        private readonly ILocationDao _locationDao;
        private readonly IDisciplineDao _disciplineDao;
        private readonly IGenderDao _genderDao;
        private readonly ISkierDao _skierDao;
        private readonly ISensorDao _sensorDao;
        private readonly ITimeDataDao _timeDataDao;
        private readonly ISeasonDao _seasonDao;

        public RaceService(IRaceDao raceDao, IDisciplineDao disciplineDao, ILocationDao locationDao,
            IStartListDao startListDao, IGenderDao genderDao, ISkierDao skierDao, ISensorDao sensorDao,
            ITimeDataDao timeDataDao, ISeasonDao seasonDao)
        {
            _raceDao = raceDao;
            _disciplineDao = disciplineDao;
            _locationDao = locationDao;
            _startListDao = startListDao;
            _genderDao = genderDao;
            _skierDao = skierDao;
            _sensorDao = sensorDao;
            _timeDataDao = timeDataDao;
            _seasonDao = seasonDao;
        }

        public Task<Race?> GetRaceById(int raceId) => _raceDao.FindByIdAsync(raceId);

        public Task<IEnumerable<Gender>?> GetGenders() => _genderDao.FindAllAsync();

        public Task<IEnumerable<Location>?> GetLocations() => _locationDao.FindAllAsync();

        public Task<IEnumerable<Discipline>?> GetDisciplines() => _disciplineDao.FindAllAsync();

        public Task<IEnumerable<Race>?> GetAllRaces() => _raceDao.FindAllAsync();

        public Task<IEnumerable<Race>?> GetRacesForSeason(int seasonId) => _raceDao.GetRaceForSeasonId(seasonId);

        public Task<IEnumerable<Season>?> GetAllSeasons() => _seasonDao.FindAllAsync();

        public Task<IEnumerable<Skier>?> GetAvailableSkiersForRace(int raceId) =>
            _skierDao.FindAvailableSkiersForRace(raceId);

        public Task<IEnumerable<StartList>?> GetStartListForRace(int raceId) =>
            _startListDao.GetStartListForRace(raceId);

        public async Task<RaceUpdateState> InsertOrUpdateRace(Race race, int sensorCount)
        {
            try
            {
                using var scope = ScopeBuilder.BuildTransactionScope();
                if (race.Id == -1)
                {
                    var raceId = await _raceDao.InsertGetIdAsync(race);
                    if (raceId.HasValue) race.Id = raceId.Value;
                    else return RaceUpdateState.Err;
                }
                else if (!await UpdateInvalid(race)) await _raceDao.UpdateAsync(race);
                else return RaceUpdateState.StartListDefined;

                var sensors = (await _sensorDao.FindAllSensorsForRace(race.Id)).ToList();

                if (sensors.Count > sensorCount)
                    foreach (var s in sensors.Where(s => s.SensorNumber >= sensorCount))
                        await _sensorDao.DeleteAsync(s.Id);
                else
                    for (var i = sensors.Count; i < sensorCount; i++)
                        await _sensorDao.InsertAsync(new Sensor {RaceId = race.Id, SensorNumber = i});

                scope.Complete();
                return RaceUpdateState.Ok;
            }
            catch (Exception)
            {
                return RaceUpdateState.Err;
            }
        }

        private async Task<bool> UpdateInvalid(Race race)
        {
            var ogRace = await _raceDao.FindByIdAsync(race.Id);
            var slDefined = await StartListDefined(race.Id);
            return slDefined && (ogRace.DisciplineId != race.DisciplineId || ogRace.GenderId != race.GenderId);
        }

        public async Task<int?> GetSensorCount(int raceId) => (await _sensorDao.FindAllSensorsForRace(raceId))?.Count();

        public async Task<bool> RemoveRace(Race race)
        {
            if (await _raceDao.FindByIdAsync(race.Id) == null ||
                (await _startListDao.CountStartListForRace(race.Id)) != 0 ||
                (await _timeDataDao.CountTimeDataForRace(race.Id) != 0))
                return false;
            await _sensorDao.DeleteAllSensorsForRace(race.Id);
            await _raceDao.DeleteAsync(race.Id);
            return true;
        }

        public async Task<IEnumerable<RaceRanking>?> GetFinishedSkierRanking(int raceId)
        {
            var maxSensorNr = await _sensorDao.GetMaxSensorNr(raceId);
            var ranking = new List<RaceRanking>();

            var position = 0;
            var equalityIncrease = 1;
            foreach (var timeData in await _timeDataDao.GetRankingForSensor(raceId, maxSensorNr.Value))
            {
                if (position != 0)
                    if (ranking[position - 1].Time == timeData.Time) equalityIncrease++;
                    else
                    {
                        position += equalityIncrease;
                        equalityIncrease = 1;
                    }
                else position++;

                ranking.Add(new RaceRanking
                {
                    StartList = timeData.StartList,
                    Time = timeData.Time,
                    Position = position,
                    TimeToLeader = timeData.Time - ranking.FirstOrDefault()?.Time ?? 0
                });
            }

            return ranking;
        }

        public Task<IEnumerable<StartList>?> GetDisqualifiedSkiers(int raceId) =>
            _startListDao.GetDisqualifiedSkierForRace(raceId);

        public Task<IEnumerable<TimeData>?> GetTimeDataForStartList(int raceId,
            int skierId) => _timeDataDao.GetTimeDataForStartList(skierId, raceId);

        public Task<IEnumerable<Discipline>?> GetDisciplinesForLocation(int locationId) =>
            _locationDao.GetPossibleDisciplinesForLocation(locationId);

        private async Task<bool> StartListDefined(int raceId) =>
            await _startListDao.CountStartListForRace(raceId) > 0;

        public async Task<IEnumerable<RaceRanking>?> GetRankingForRace(int raceId)
        {
            var ranking = (await GetFinishedSkierRanking(raceId)).ToList();
            ranking.AddRange((await GetDisqualifiedSkiers(raceId))
                             .Select(sl => new RaceRanking
                             {
                                 StartList = sl
                             }));
            return ranking;
        }

        public async Task<bool> UpdateStartList(Race race, IEnumerable<StartList> startList)
        {
            await _startListDao.DeleteAllForRace(race.Id);
            foreach (var sl in startList) await _startListDao.InsertAsync(sl);

            return true;
        }

        public async Task<TimeSpan?> GetDifferenceToLeader(TimeData timeData)
        {
            var maxSensorNr = await _sensorDao.GetMaxSensorNr(timeData.RaceId);
            if (maxSensorNr == null) return null;
            var leader = 
                (await _timeDataDao.GetRankingForSensor(timeData.RaceId, maxSensorNr.Value, 1)).FirstOrDefault();
            if (leader == null) return TimeSpan.Zero; //no leader
            var leaderTime = await _timeDataDao.FindByIdAsync(leader.SkierId, leader.RaceId, timeData.SensorId);

            if (leaderTime == null) return null; //no leader time
            return TimeSpan.FromMilliseconds(timeData.Time - leaderTime.Time);
        }

        public async Task<IEnumerable<TimeDifference>?> GetTimeDataForSkierWithDifference(int skierId, int raceId)
        {
            var timeDataList = await _timeDataDao.GetTimeDataForStartList(skierId, raceId);
            var retVal = new List<TimeDifference>();
            foreach (var timeData in timeDataList)
            {
                var res = await GetDifferenceToLeader(timeData);
                if (!res.HasValue) continue;
                retVal.Add(new TimeDifference
                {
                    TimeData = timeData,
                    DifferenceToLeader = (int)res.Value.TotalMilliseconds
                });
            }

            return retVal;
        }

        public async Task<DateTime?> GetStartTimeForSkier(int skierId, int raceId)
        {
            var sensor = await _sensorDao.GetSensorForSensorNumber(0, raceId);
            return (await _timeDataDao.FindByIdAsync(skierId, raceId, sensor.Id))
                   ?.SkierEvent.RaceData.EventDateTime;
        }

        public async Task<bool?> IsStartListDefined(int raceId) =>
            (await _startListDao.CountStartListForRace(raceId) ?? 0) != 0;
    }
}