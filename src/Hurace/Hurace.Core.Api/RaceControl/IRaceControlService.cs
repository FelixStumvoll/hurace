using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Api.RaceControl.Events;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceControl
{
    public interface IRaceControlService
    {
        event Action<TimeData> OnTimeData;
        event Action<Event> OnEvent;
        Task<bool> StartRace(Race race);
        Task<IEnumerable<StartList>> GetStartListForRace(int raceId);
        Task<IEnumerable<TimeData>> GetTimeDataForStartList(int raceId, int skierId);
        Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId);
        Task EnableRaceForSkier(Race race);
        void CancelSkier(Skier skier);
        void CancelRace();
    }
}