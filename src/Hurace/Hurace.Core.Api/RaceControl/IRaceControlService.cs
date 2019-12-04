using System;
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
        Task EnableRaceForSkier(Race race);
        void CancelSkier(Skier skier);
        void CancelRace();
    }
}