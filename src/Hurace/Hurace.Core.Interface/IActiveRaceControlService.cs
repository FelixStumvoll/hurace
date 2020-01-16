using System;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Interface
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
        Task<bool> CancelSkier(int skierId);
        Task<bool> DisqualifyCurrentSkier();
        Task<bool> DisqualifyFinishedSkier(int skierId);
        Task<bool> CancelRace();
    }
}