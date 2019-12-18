using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface ISensorDao : IDefaultCrudDao<Sensor>
    {
        Task<IEnumerable<Sensor>?> FindAllSensorsForRace(int raceId);
        Task<bool> DeleteAllSensorsForRace(int raceId);
        Task<int?> GetLastSensorNumber(int raceId);
        Task<Sensor> GetSensorForSensorNumber(int sensorNumber, int raceId);
    }
}