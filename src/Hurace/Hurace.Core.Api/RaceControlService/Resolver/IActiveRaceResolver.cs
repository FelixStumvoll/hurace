using System.Threading.Tasks;

namespace Hurace.Core.Api.RaceControlService
{
    public interface IActiveRaceResolver
    {
        Task<IActiveRaceControlService?> StartRace(int raceId); 
        IActiveRaceControlService this[int raceId] { get; }
        Task<bool> EndRace(int raceId);
    }
}