using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Api.Race
{
    internal class RaceService : IRaceService
    {
        private readonly IRaceDao _raceDao;
        private readonly IStartListDao _startListDao;
        private readonly ILocationDao _locationDao;
        private readonly IDisciplineDao _disciplineDao;
        private readonly IGenderDao _genderDao;
        private readonly ISkierDao _skierDao;

        public RaceService(IRaceDao raceDao, IDisciplineDao disciplineDao, ILocationDao locationDao,
            IStartListDao startListDao, IGenderDao genderDao, ISkierDao skierDao)
        {
            _raceDao = raceDao;
            _disciplineDao = disciplineDao;
            _locationDao = locationDao;
            _startListDao = startListDao;
            _genderDao = genderDao;
            _skierDao = skierDao;
        }

        public Task<IEnumerable<Gender>> GetGenders() => _genderDao.FindAllAsync();

        public Task<IEnumerable<Location>> GetLocations() => _locationDao.FindAllAsync();

        public Task<IEnumerable<Discipline>> GetDisciplines() => _disciplineDao.FindAllAsync();

        public Task<IEnumerable<Dal.Domain.Race>> GetAllRaces() => _raceDao.FindAllAsync();

        public Task<IEnumerable<Dal.Domain.Race>> GetActiveRaces() => _raceDao.GetActiveRaces();

        public Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId) =>
            _skierDao.FindAvailableSkiersForRace(raceId);

        public Task<IEnumerable<StartList>> GetStartListForRace(int raceId) =>
            _startListDao.GetStartListForRace(raceId);
    }
}