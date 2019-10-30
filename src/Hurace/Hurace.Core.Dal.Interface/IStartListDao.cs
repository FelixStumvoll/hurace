using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface IStartListDao : IBaseDao<StartList>
    {
        Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
        Task<StartList?> GetCurrentSkierForRace(int raceId);
        Task<StartList?> GetNextSkierForRace(int raceId);
        Task<bool> DeleteAsync(int raceId, int skierId);
    }
}