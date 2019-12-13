using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Api.Util;
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
        Task<Result<bool,Exception>> EnableRaceForSkier();
        Task<Result<StartList, Exception>> GetCurrentSkier();
        Task<Result<bool,Exception>> CancelSkier(int skierId);
        Task<Result<IEnumerable<StartList>, Exception>> GetRemainingStartList();
        Task<Result<TimeSpan?, Exception>> GetDifferenceToLeader(TimeData timeData);
        Task<Result<bool,Exception>> CancelRace();
    }
}