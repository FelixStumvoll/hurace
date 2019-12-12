using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceControlService.Service
{
    public interface IActiveRaceControlService
    {
        int RaceId { get; set; }
        event Action<TimeData> OnTimeData;
        event Action<StartList> OnSkierStarted;
        event Action<StartList> OnSkierFinished;
        event Action<StartList> OnSkierCanceled;
        event Action<StartList> OnCurrentSkierDisqualified;
        event Action<StartList> OnLateDisqualification;
        event Action<TimeData> OnSplitTime;
        event Action OnRaceCanceled;
        event Action OnRaceFinished;
        
        Task InitializeAsync();
        Task<bool> EnableRaceForSkier();
        Task<StartList> GetCurrentSkier();
        Task<bool> CancelSkier(int skierId);
        Task<IEnumerable<StartList>?> GetRemainingStartList();
        Task<TimeSpan?> GetDifferenceToLeader(TimeData timeData);
        Task<bool> CancelRace();
    }
}