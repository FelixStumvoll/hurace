using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Service
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

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Race>> GetRacesForSeason(int seasonId) => _raceDao.GetRacesForSeasonId(seasonId);

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Season>> GetAllSeasons() => _seasonDao.FindAllAsync();

        [ExcludeFromCodeCoverage]
        public Task<Season?> GetSeasonById(int seasonId) => _seasonDao.FindByIdAsync(seasonId);

        private static bool SeasonValidator(Season season) => season.StartDate < season.EndDate;

        [ExcludeFromCodeCoverage]
        public async Task<int?> InsertSeason(Season season) =>
            SeasonValidator(season) ? await _seasonDao.InsertGetIdAsync(season) : null;

        public async Task<bool> DeleteSeason(int id)
        {
            if (await _seasonDao.CountRacesForSeason(id) != 0) return false;
            return await _seasonDao.DeleteAsync(id);
        }

        [ExcludeFromCodeCoverage]
        public async Task<bool> UpdateSeason(Season season) =>
            SeasonValidator(season) && await _seasonDao.UpdateAsync(season);
    }
}