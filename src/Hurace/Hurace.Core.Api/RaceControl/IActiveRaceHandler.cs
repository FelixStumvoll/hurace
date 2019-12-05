using System.Threading.Tasks;

namespace Hurace.Core.Api.RaceControl
{
    public interface IActiveRaceHandler
    {
        Task<IRaceControlService> StartRace(int raceId); 
        IRaceControlService GetRaceControlServiceForRace(int raceId);
        Task EndRace(int raceId);
    }
}