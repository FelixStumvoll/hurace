using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.ActiveRaceControlService.Service
{
    public interface IActiveRaceControlService
    {
        int RaceId { get; set; }
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
        Task<StartList?> GetCurrentSkier();
        Task<bool> CancelSkier(int skierId);
        Task<IEnumerable<StartList>?> GetRemainingStartList();
        Task<bool> DisqualifyCurrentSkier();
        Task<bool> DisqualifyFinishedSkier(int skierId);
        Task<int?> GetPossiblePositionForCurrentSkier();
        Task<bool> CancelRace();
    }
}