using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface IRaceDao : IBaseDao<Race>
    {
        Task<IEnumerable<RaceRanking>> GetRanking(int raceId);
        Task<IEnumerable<StartList>> GetStartList(int raceId);
    }
}