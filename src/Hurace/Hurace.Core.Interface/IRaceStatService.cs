﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Interface.Entities;
using Hurace.Dal.Domain;

namespace Hurace.Core.Interface
{
    public interface IRaceStatService
    {
        Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId);
        Task<IEnumerable<RaceRanking>?> GetFinishedSkierRanking(int raceId, int count = 0);
        Task<IEnumerable<StartList>> GetDisqualifiedSkiers(int raceId);
        Task<IEnumerable<TimeData>> GetTimeDataForStartList(int raceId, int skierId);
        Task<TimeSpan?> GetDifferenceToLeader(TimeData timeData);
        Task<IEnumerable<TimeDifference>> GetTimeDataForSkierWithDifference(int skierId, int raceId);
        Task<DateTime?> GetStartTimeForSkier(int skierId, int raceId);
        Task<IEnumerable<RaceRanking>?> GetWinnersForRace(int raceId);
    }
}