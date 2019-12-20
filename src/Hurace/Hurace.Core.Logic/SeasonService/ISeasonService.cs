using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Logic.SeasonService
{
    public interface ISeasonService
    {
        Task<IEnumerable<Race>> GetRacesForSeason(int seasonId);
        Task<IEnumerable<Season>> GetAllSeasons();
    }
}