using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface IStartListDao : ICrudDao<StartList>
    {
        Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
        Task<StartList?> GetCurrentSkierForRace(int raceId);
        Task<StartList?> GetNextSkierForRace(int raceId);
        Task<StartList?> FindByIdAsync(int skierId, int raceId);
        Task<bool> DeleteAsync(int raceId, int skierId);
    }
}