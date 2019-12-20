using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.RaceService;
using Hurace.Dal.Domain;

namespace Hurace.Core.Logic.RaceBaseDataService
{
    public interface IRaceBaseDataService
    {
        Task<Race?> GetRaceById(int raceId);
        Task<RaceUpdateState> InsertOrUpdateRace(Race race, int sensorCount);
        Task<int?> GetSensorCount(int raceId);
        Task<bool> RemoveRace(Race race);
        Task<IEnumerable<Gender>> GetGenders();
        Task<IEnumerable<Location>> GetLocations();
        Task<IEnumerable<Discipline>> GetDisciplinesForLocation(int locationId);
    }
}