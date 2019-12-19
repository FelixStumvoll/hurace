using System.Threading.Tasks;
using Hurace.Core.Api.ActiveRaceControlService.Service;

namespace Hurace.Core.Api.ActiveRaceControlService.Resolver
{
    public interface IActiveRaceResolver
    {
        Task<IActiveRaceControlService> StartRace(int raceId);
        IActiveRaceControlService this[int raceId] { get; }
        Task<bool> EndRace(int raceId);
    }
}