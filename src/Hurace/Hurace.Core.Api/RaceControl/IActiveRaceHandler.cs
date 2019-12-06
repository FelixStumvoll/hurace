using System.Threading.Tasks;

namespace Hurace.Core.Api.RaceControl
{
    public interface IActiveRaceHandler
    {
        Task<IRaceControlService> StartRace(int raceId); 
        IRaceControlService this[int raceId] { get; }
        Task EndRace(int raceId);
    }
}