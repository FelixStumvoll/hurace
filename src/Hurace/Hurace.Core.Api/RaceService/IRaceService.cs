using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Api.Util;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceService
{
    public interface IRaceService
    {
        Task<Result<Race, Exception>> GetRaceById(int raceId);
        Task<Result<IEnumerable<Gender>, Exception>> GetGenders();
        Task<Result<IEnumerable<Location>, Exception>> GetLocations();
        Task<Result<IEnumerable<Discipline>,Exception>> GetDisciplines();
        Task<Result<IEnumerable<Race>,Exception>> GetAllRaces();
        Task<Result<IEnumerable<Race>, Exception>> GetRacesForSeason(int seasonId);
        Task<Result<IEnumerable<Season>, Exception>> GetAllSeasons();
        Task<Result<IEnumerable<Skier>, Exception>> GetAvailableSkiersForRace(int raceId);
        Task<Result<IEnumerable<StartList>, Exception>> GetStartListForRace(int raceId);
        Task<RaceUpdateState> InsertOrUpdateRace(Race race, int sensorCount);
        Task<Result<int, Exception>> GetSensorCount(int raceId);
        Task<Result<bool,Exception>> RemoveRace(Race race);
        Task<Result<bool,Exception>> UpdateStartList(Race race, IEnumerable<StartList> startList);
        Task<Result<IEnumerable<RaceRanking>, Exception>> GetRankingForRace(int raceId);
        Task<Result<IEnumerable<TimeData>, Exception>> GetTimeDataForStartList(int raceId, int skierId);
        Task<Result<IEnumerable<Discipline>, Exception>> GetDisciplinesForLocation(int locationId);
    }
}