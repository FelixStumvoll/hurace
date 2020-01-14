using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Race>> GetRacesForSeason(int seasonId) => _raceDao.GetRacesForSeasonId(seasonId);

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Season>> GetAllSeasons() => _seasonDao.FindAllAsync();

        [ExcludeFromCodeCoverage]
        public Task<Season?> GetSeasonById(int seasonId) => _seasonDao.FindByIdAsync(seasonId);

        private bool SeasonValidator(Season season) => season.StartDate < season.EndDate;

        [ExcludeFromCodeCoverage]
        public Task<int?> InsertSeason(Season season) =>
            SeasonValidator(season) ? _seasonDao.InsertGetIdAsync(season) : null;

        public async Task<bool> DeleteSeason(int id)
        {
            if (await _seasonDao.CountRacesForSeason(id) != 0) return false;
            return await _seasonDao.DeleteAsync(id);
        }

        [ExcludeFromCodeCoverage]
        public Task<bool> UpdateSeason(Season season) =>
            SeasonValidator(season) ? _seasonDao.UpdateAsync(season) : null;
    }
}