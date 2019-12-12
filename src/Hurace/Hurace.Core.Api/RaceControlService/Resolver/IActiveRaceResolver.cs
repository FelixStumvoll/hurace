using System.Threading.Tasks;
using Hurace.Core.Api.RaceControlService.Service;

namespace Hurace.Core.Api.RaceControlService.Resolver
{
    public interface IActiveRaceResolver
    {
        Task<IActiveRaceControlService?> StartRace(int raceId); 
        IActiveRaceControlService this[int raceId] { get; }
        Task<bool> EndRace(int raceId);
    }
}