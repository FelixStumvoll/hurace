using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface IRaceDao : IDefaultCrudDao<Race>
    {
        Task<IEnumerable<Race>> GetActiveRaces();
        Task<Race?> GetActiveRaceById(int raceId);
        Task<IEnumerable<Race>> GetRacesForSeasonId(int seasonId);
        
    }
}