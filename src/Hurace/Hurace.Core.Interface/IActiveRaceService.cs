using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Interface.Entities;
using Hurace.Dal.Domain;

namespace Hurace.Core.Interface
{
    public interface IActiveRaceService
    {
        Task<StartList?> GetCurrentSkier(int raceId);
        Task<IEnumerable<StartList>?> GetRemainingStartList(int raceId);
        Task<int?> GetPossiblePositionForCurrentSkier(int raceId);
        Task<IEnumerable<Race>> GetActiveRaces();
        Task<IEnumerable<TimeDifference>?> GetSplitTimesForCurrentSkier(int raceId);
    }
}