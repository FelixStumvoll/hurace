using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface ITimeDataDao : ICrudDao<TimeData>
    {
        Task<IEnumerable<TimeData>> GetRankingForSensor(int raceId, int sensorNumber, int count = 0);
        Task<bool> DeleteAsync(int skierId, int raceId, int sensorId);
        Task<TimeData?> FindByIdAsync(int skierId, int raceId, int sensorId);
        Task<IEnumerable<TimeData>> GetTimeDataForStartList(int skierId, int raceId);
        Task<int?> CountTimeDataForRace(int raceId);
        Task<int?> GetAverageTimeForSensor(int raceId, int sensorNumber);
        Task<DateTime?> GetStartTimeForStartList(int skierId, int raceId);
        Task<IEnumerable<TimeData>> GetRankingForSkier(int skierId);
    }
}