using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Interface
{
    public interface IRaceBaseDataService
    {
        Task<IEnumerable<Race>> GetAllRaces();
        Task<Race?> GetRaceById(int raceId);
        Task<RaceModificationResult> InsertOrUpdateRace(Race race, int sensorCount);
        Task<int?> GetSensorCount(int raceId);
        Task<bool> RemoveRace(int raceId);
        Task<IEnumerable<Gender>> GetGenders();
        Task<IEnumerable<Location>> GetLocations();
        Task<IEnumerable<Discipline>> GetDisciplinesForLocation(int locationId);
        Task<IEnumerable<Discipline>> GetAllDisciplines();
    }
}