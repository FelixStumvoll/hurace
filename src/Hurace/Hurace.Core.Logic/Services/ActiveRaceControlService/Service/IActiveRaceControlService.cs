using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Logic.Services.ActiveRaceControlService.Service
{
    public interface IActiveRaceControlService
    {
        event Action<StartList> OnSkierStarted;
        event Action<StartList> OnSkierFinished;
        event Action<StartList> OnSkierCancelled;
        event Action<StartList> OnCurrentSkierDisqualified;
        event Action<StartList> OnLateDisqualification;
        event Action<TimeData> OnSplitTime;
        event Action<Race> OnRaceCancelled;
        event Action<Race> OnRaceFinished;
        public int RaceId { get; }

        Task<bool> StartRace();
        Task<bool> EndRace();
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