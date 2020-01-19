using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Interface
{
    public interface IRaceService
    {
        Task<IEnumerable<Race>> GetAllRaces();
        Task<Race?> GetRaceById(int raceId);
        Task<RaceModificationResult> InsertOrUpdateRace(Race race, int sensorCount);
        Task<int?> GetSensorCount(int raceId);
        Task<bool> RemoveRace(int raceId);
    }
}