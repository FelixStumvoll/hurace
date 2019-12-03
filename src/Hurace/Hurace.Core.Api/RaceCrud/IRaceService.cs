using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceCrud
{
    public interface IRaceService
    {
        Task<IEnumerable<Gender>> GetGenders();
        Task<IEnumerable<Location>> GetLocations();
        Task<IEnumerable<Discipline>> GetDisciplines();
        Task<IEnumerable<Race>> GetAllRaces();
        Task<IEnumerable<Race>> GetActiveRaces();
        Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId);
        Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
        Task<bool> InsertOrUpdateRace(Race race, int sensorCount);
        Task<int> GetSensorCount(int raceId);
        Task<bool> RemoveRace(Race race);
        Task<bool> UpdateStartList(Race race, IEnumerable<StartList> startList);
    }
}