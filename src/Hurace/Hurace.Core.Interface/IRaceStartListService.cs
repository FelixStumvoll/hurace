using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Interface
{
    public interface IRaceStartListService
    {
        Task<bool> UpdateStartList(int raceId, IEnumerable<StartList> startList);
        Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId);
        Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
        Task<bool?> IsStartListDefined(int raceId);
        Task<StartList?> GetStartListById(int skierId, int raceId);
    }
}