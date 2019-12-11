using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        public Task<Dal.Domain.Race> GetRaceById(int raceId) => _raceDao.FindByIdAsync(raceId);

        public Task<IEnumerable<Gender>> GetGenders() => _genderDao.FindAllAsync();

        public Task<IEnumerable<Location>> GetLocations() => _locationDao.FindAllAsync();

        public Task<IEnumerable<Discipline>> GetDisciplines() => _disciplineDao.FindAllAsync();

        public Task<IEnumerable<Dal.Domain.Race>> GetAllRaces() => _raceDao.FindAllAsync();
        public Task<IEnumerable<Dal.Domain.Race>> GetRacesForSeason(int seasonId) => _raceDao.GetRaceForSeasonId(seasonId);

        public Task<IEnumerable<Season>> GetAllSeasons() => _seasonDao.FindAllAsync();

        public Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId) =>
            _skierDao.FindAvailableSkiersForRace(raceId);

        public Task<IEnumerable<StartList>> GetStartListForRace(int raceId) =>
            _startListDao.GetStartListForRace(raceId);

        public async Task<bool> InsertOrUpdateRace(Dal.Domain.Race race, int sensorCount)
        {
            if (race.Id == -1) race.Id = await _raceDao.InsertGetIdAsync(race);
            else await _raceDao.UpdateAsync(race);

            var sensors = (await _sensorDao.FindAllSensorsForRace(race.Id)).ToList();

            if (sensors.Count > sensorCount)
                foreach (var s in sensors.Where(s => s.SensorNumber >= sensorCount))
                    await _sensorDao.DeleteAsync(s.Id);
            else
                for (var i = sensors.Count; i < sensorCount; i++)
                    await _sensorDao.InsertAsync(new Sensor {RaceId = race.Id, SensorNumber = i});

            return true;
        }

        public async Task<int> GetSensorCount(int raceId) => (await _sensorDao.FindAllSensorsForRace(raceId)).Count();

        public async Task<bool> RemoveRace(Dal.Domain.Race race)
        {
            if (await _raceDao.FindByIdAsync(race.Id) == null ||
                (await _startListDao.CountStartListForRace(race.Id)) != 0 ||
                (await _timeDataDao.CountTimeDataForRace(race.Id) != 0))
                return false;
            await _sensorDao.DeleteAllSensorsForRace(race.Id);
            await _raceDao.DeleteAsync(race.Id);
            return true;
        }

        public Task<IEnumerable<TimeData>> GetTimeDataForStartList(int raceId, int skierId) =>
            _timeDataDao.GetTimeDataForStartList(skierId, raceId);

        public async Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId)
        {
            var timeRanking = await _timeDataDao.GetRankingForRace(raceId);
            var disqualifiedSkiers = (await _startListDao.GetDisqualifiedSkierForRace(raceId)).Select(
                sl => new RaceRanking
                {
                    Skier = sl.Skier,
                    StartList = sl,
                    RaceId = raceId, RaceTime = DateTime.MinValue
                });

            return timeRanking.Concat(disqualifiedSkiers);
        }

        public async Task<bool> UpdateStartList(Dal.Domain.Race race, IEnumerable<StartList> startList)
        {
            await _startListDao.DeleteAllForRace(race.Id);
            foreach (var sl in startList) await _startListDao.InsertAsync(sl);

            return true;
        }
    }
}