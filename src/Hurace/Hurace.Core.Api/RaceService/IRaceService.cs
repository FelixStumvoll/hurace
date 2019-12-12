using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceService
{
    public interface IRaceService
    {
        Task<Race> GetRaceById(int raceId);
        Task<IEnumerable<Gender>?> GetGenders();
        Task<IEnumerable<Location>?> GetLocations();
        Task<IEnumerable<Discipline>?> GetDisciplines();
        Task<IEnumerable<Race>?> GetAllRaces();
        Task<IEnumerable<Race>?> GetRacesForSeason(int seasonId);
        Task<IEnumerable<Season>?> GetAllSeasons();
        Task<IEnumerable<Skier>?> GetAvailableSkiersForRace(int raceId);
        Task<IEnumerable<StartList>?> GetStartListForRace(int raceId);
        Task<RaceUpdateState> InsertOrUpdateRace(Race race, int sensorCount);
        Task<int?> GetSensorCount(int raceId);
        Task<bool> RemoveRace(Race race);
        Task<bool> UpdateStartList(Race race, IEnumerable<StartList> startList);
        Task<IEnumerable<RaceRanking>?> GetRankingForRace(int raceId);
        Task<IEnumerable<TimeData>?> GetTimeDataForStartList(int raceId, int skierId);
        Task<IEnumerable<Discipline>?> GetDisciplinesForLocation(int locationId);
        Task<bool> StartListDefined(int raceId);
    }
}