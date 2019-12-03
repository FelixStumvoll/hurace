using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Api.RaceCrud
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

        public RaceService(IRaceDao raceDao, IDisciplineDao disciplineDao, ILocationDao locationDao,
            IStartListDao startListDao, IGenderDao genderDao, ISkierDao skierDao, ISensorDao sensorDao)
        {
            _raceDao = raceDao;
            _disciplineDao = disciplineDao;
            _locationDao = locationDao;
            _startListDao = startListDao;
            _genderDao = genderDao;
            _skierDao = skierDao;
            _sensorDao = sensorDao;
        }

        public Task<IEnumerable<Gender>> GetGenders() => _genderDao.FindAllAsync();

        public Task<IEnumerable<Location>> GetLocations() => _locationDao.FindAllAsync();

        public Task<IEnumerable<Discipline>> GetDisciplines() => _disciplineDao.FindAllAsync();

        public Task<IEnumerable<Race>> GetAllRaces() => _raceDao.FindAllAsync();

        public Task<IEnumerable<Race>> GetActiveRaces() => _raceDao.GetActiveRaces();

        public Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId) =>
            _skierDao.FindAvailableSkiersForRace(raceId);

        public Task<IEnumerable<StartList>> GetStartListForRace(int raceId) =>
            _startListDao.GetStartListForRace(raceId);

        public async Task<bool> InsertOrUpdateRace(Race race, int sensorCount)
        {
            race.SeasonId = 1; //TODO fix this shit
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

        public async Task<bool> RemoveRace(Race race)
        {
            if (await _raceDao.FindByIdAsync(race.Id) == null) return false;
            await _raceDao.DeleteAsync(race.Id);
            return true;
        }

        public async Task<bool> UpdateStartList(Race race, IEnumerable<StartList> startList)
        {
            await _startListDao.DeleteAllForRace(race.Id);
            foreach (var sl in startList) await _startListDao.InsertAsync(sl);

            return true;
        }
    }
}