using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Logic.Services.SeasonService
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
        public Task<Season?> GetSeasonById(int seasonId) => _seasonDao.FindByIdAsync(seasonId);
        public Task<int?> InsertSeason(Season season) => _seasonDao.InsertGetIdAsync(season);
        public async Task<bool> DeleteSeason(int id)
        {
            if (await _seasonDao.CountRacesForSeason(id) != 0) return false;
            await _seasonDao.DeleteAsync(id);
            return true;
        }

        public Task<bool> UpdateSeason(Season season) => _seasonDao.UpdateAsync(season);
    }
}