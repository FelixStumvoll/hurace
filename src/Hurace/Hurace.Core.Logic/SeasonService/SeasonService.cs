using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Logic.SeasonService
{
    public class SeasonService : ISeasonService
    {
        private readonly IRaceDao _raceDao;
        private readonly ISeasonDao _seasonDao;

        public SeasonService(ISeasonDao seasonDao, IRaceDao raceDao)
        {
            _seasonDao = seasonDao;
            _raceDao = raceDao;
        }

        public Task<IEnumerable<Race>> GetRacesForSeason(int seasonId) => _raceDao.GetRacesForSeasonId(seasonId);

        public Task<IEnumerable<Season>> GetAllSeasons() => _seasonDao.FindAllAsync();
    }
}