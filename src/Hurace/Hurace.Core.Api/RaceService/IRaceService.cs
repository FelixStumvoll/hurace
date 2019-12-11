using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceService
{
    public interface IRaceService
    {
        Task<Dal.Domain.Race> GetRaceById(int raceId);
        Task<IEnumerable<Gender>> GetGenders();
        Task<IEnumerable<Location>> GetLocations();
        Task<IEnumerable<Discipline>> GetDisciplines();
        Task<IEnumerable<Dal.Domain.Race>> GetAllRaces();
        Task<IEnumerable<Dal.Domain.Race>> GetRacesForSeason(int seasonId);
        Task<IEnumerable<Season>> GetAllSeasons();
        Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId);
        Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
        Task<bool> InsertOrUpdateRace(Dal.Domain.Race race, int sensorCount);
        Task<int> GetSensorCount(int raceId);
        Task<bool> RemoveRace(Dal.Domain.Race race);
        Task<bool> UpdateStartList(Dal.Domain.Race race, IEnumerable<StartList> startList);
        Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId);
        Task<IEnumerable<TimeData>> GetTimeDataForStartList(int raceId, int skierId);
    }
}