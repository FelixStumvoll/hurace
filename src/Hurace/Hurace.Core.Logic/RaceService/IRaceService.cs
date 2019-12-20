using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Dal.Domain;

namespace Hurace.Core.Logic.RaceService
{
    public interface IRaceService
    {
        Task<Race?> GetRaceById(int raceId);
        Task<IEnumerable<Gender>> GetGenders();
        Task<IEnumerable<Location>> GetLocations();
        Task<IEnumerable<Discipline>> GetDisciplinesForLocation(int locationId);
        // Task<IEnumerable<Race>> GetRacesForSeason(int seasonId);
        // Task<IEnumerable<Season>> GetAllSeasons();
       
        Task<StartList?> GetStartListById(int skierId, int raceId);
        //BASEDATA
        // Task<RaceUpdateState> InsertOrUpdateRace(Race race, int sensorCount); //moved
        // Task<int?> GetSensorCount(int raceId); //moved
        // Task<bool> RemoveRace(Race race); //moved
        // //STARTLIST
        // Task<bool> UpdateStartList(Race race, IEnumerable<StartList> startList);
        // Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId);
        // Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
        // Task<bool?> IsStartListDefined(int raceId);
        // //STATSERVICE
        // Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId);
        // Task<IEnumerable<RaceRanking>> GetFinishedSkierRanking(int raceId);
        //
        // Task<IEnumerable<StartList>> GetDisqualifiedSkiers(int raceId);
        // Task<IEnumerable<TimeData>> GetTimeDataForStartList(int raceId, int skierId);
        //
        // Task<TimeSpan?> GetDifferenceToLeader(TimeData timeData);
        // Task<IEnumerable<TimeDifference>> GetTimeDataForSkierWithDifference(int skierId, int raceId);
        // Task<DateTime?> GetStartTimeForSkier(int skierId, int raceId);
        
    }
}