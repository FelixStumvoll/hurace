using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using static Hurace.Core.Api.ExceptionWrapper;

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


        public async Task<Race> GetRaceById(int raceId) => await Try(() => _raceDao.FindByIdAsync(raceId));

        public async Task<IEnumerable<Gender>?> GetGenders() => await Try(_genderDao.FindAllAsync);

        public async Task<IEnumerable<Location>?> GetLocations() => await Try(_locationDao.FindAllAsync);

        public async Task<IEnumerable<Discipline>?> GetDisciplines() => 
            await Try(_disciplineDao.FindAllAsync);

        public async Task<IEnumerable<Race>?> GetAllRaces() => 
            await Try(_raceDao.FindAllAsync);

        public async Task<IEnumerable<Race>?> GetRacesForSeason(int seasonId) => 
            await Try(async () => await _raceDao.GetRaceForSeasonId(seasonId));

        public async Task<IEnumerable<Season>?> GetAllSeasons() => 
            await Try(_seasonDao.FindAllAsync);

        public async Task<IEnumerable<Skier>?> GetAvailableSkiersForRace(int raceId) => 
            await Try(() => _skierDao.FindAvailableSkiersForRace(raceId));

        public async Task<IEnumerable<StartList>?> GetStartListForRace(int raceId) => 
            await Try(() => _startListDao.GetStartListForRace(raceId));

        public async Task<RaceUpdateState> InsertOrUpdateRace(Race race, int sensorCount)
        {
            try
            {
                if (race.Id == -1) race.Id = await _raceDao.InsertGetIdAsync(race);
                else if (!await UpdateInvalid(race)) await _raceDao.UpdateAsync(race);
                else return RaceUpdateState.StartListDefined;

                var sensors = (await _sensorDao.FindAllSensorsForRace(race.Id)).ToList();

                if (sensors.Count > sensorCount)
                    foreach (var s in sensors.Where(s => s.SensorNumber >= sensorCount))
                        await _sensorDao.DeleteAsync(s.Id);
                else
                    for (var i = sensors.Count; i < sensorCount; i++)
                        await _sensorDao.InsertAsync(new Sensor {RaceId = race.Id, SensorNumber = i});

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

        public async Task<int?> GetSensorCount(int raceId) => 
            await Try(async () => (await _sensorDao.FindAllSensorsForRace(raceId)).Count());

        public async Task<bool> RemoveRace(Race race) =>
            await Try(async () =>
            {
                if (await _raceDao.FindByIdAsync(race.Id) == null ||
                    (await _startListDao.CountStartListForRace(race.Id)) != 0 ||
                    (await _timeDataDao.CountTimeDataForRace(race.Id) != 0))
                    return false;
                await _sensorDao.DeleteAllSensorsForRace(race.Id);
                await _raceDao.DeleteAsync(race.Id);
                return true;
            });

        public async Task<IEnumerable<TimeData>?> GetTimeDataForStartList(int raceId, int skierId) => 
            await Try(() => _timeDataDao.GetTimeDataForStartList(skierId, raceId));

        public async Task<IEnumerable<Discipline>?> GetDisciplinesForLocation(int locationId) => 
            await Try(async () => await _locationDao.GetPossibleDisciplinesForLocation(locationId));

        private async Task<bool> StartListDefined(int raceId) =>
            await _startListDao.CountStartListForRace(raceId) > 0;

        public async Task<IEnumerable<RaceRanking>?> GetRankingForRace(int raceId) =>
            await Try(async () =>
            {
                var timeRanking = await _timeDataDao.GetRankingForRace(raceId);
                var disqualifiedSkiers =
                    (await _startListDao.GetDisqualifiedSkierForRace(raceId))
                    .Select(sl => new RaceRanking
                    {
                        Skier = sl.Skier,
                        StartList = sl,
                        RaceId = raceId, RaceTime = DateTime.MinValue
                    });

                return timeRanking.Concat(disqualifiedSkiers);
            });

        public async Task<bool> UpdateStartList(Race race, IEnumerable<StartList> startList) =>
            await Try(async () =>
            {
                await _startListDao.DeleteAllForRace(race.Id);
                foreach (var sl in startList) await _startListDao.InsertAsync(sl);

                return true;
            });
    }
}