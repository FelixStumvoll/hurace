using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface ITimeDataDao : IBaseDao<TimeData>
    {
        Task<IEnumerable<TimeData>> GetRankingForRace(int raceId);
        Task<bool> DeleteAsync(int skierId, int raceId, int sensorId);
        Task<IEnumerable<TimeData>> FindAllAsync();
        Task<TimeData?> FindByIdAsync(int skierId, int raceId, int sensorId);
    }
}