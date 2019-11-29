using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.Race
{
    public interface IRaceService
    {
        Task<IEnumerable<Gender>> GetGenders();
        Task<IEnumerable<Location>> GetLocations();
        Task<IEnumerable<Discipline>> GetDisciplines();
        Task<IEnumerable<Dal.Domain.Race>> GetAllRaces();
        Task<IEnumerable<Dal.Domain.Race>> GetActiveRaces();
        Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId);
        Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
    }
}