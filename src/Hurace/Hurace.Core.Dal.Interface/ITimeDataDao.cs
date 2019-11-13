using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface ITimeDataDao : ICrudDao<TimeData>
    {
        Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId, int count = 0);
        Task<bool> DeleteAsync(int skierId, int raceId, int sensorId);
        Task<TimeData?> FindByIdAsync(int skierId, int raceId, int sensorId);
    }
}