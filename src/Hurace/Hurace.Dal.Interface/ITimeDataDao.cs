using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface ITimeDataDao : ICrudDao<TimeData>
    {
        Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId, int count = 0);
        Task<IEnumerable<TimeData>> GetRankingForSensor(int raceId, int sensorId, int count = 0);
        Task<bool> DeleteAsync(int skierId, int raceId, int sensorId);
        Task<TimeData?> FindByIdAsync(int skierId, int raceId, int sensorId);
        Task<IEnumerable<TimeData>> GetTimeDataForStartList(int skierId, int raceId);
        Task<TimeData?> GetTimeDataForSensor(int skierId, int raceId, int sensorId);
        Task<int> CountTimeDataForRace(int raceId);
    }
}