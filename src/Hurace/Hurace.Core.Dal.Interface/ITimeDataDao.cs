using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface ITimeDataDao : IBaseDao<TimeData>
    {
        Task<IEnumerable<TimeData>> GetRankingForRace(int raceId);
    }
}