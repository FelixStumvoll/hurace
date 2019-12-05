using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceCrud
{
    public interface IRaceService
    {
        event Action<Race> OnRaceAdded;
        event Action<Race> OnRaceUpdated;
        Task<Race> GetRaceById(int raceId);
        Task<IEnumerable<Gender>> GetGenders();
        Task<IEnumerable<Location>> GetLocations();
        Task<IEnumerable<Discipline>> GetDisciplines();
        Task<IEnumerable<Race>> GetAllRaces();
        Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId);
        Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
        Task<bool> InsertOrUpdateRace(Race race, int sensorCount);
        Task<int> GetSensorCount(int raceId);
        Task<bool> RemoveRace(Race race);
        Task<bool> UpdateStartList(Race race, IEnumerable<StartList> startList);
        Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId);
        Task<IEnumerable<TimeData>> GetTimeDataForStartList(int raceId, int skierId);
    }
}