using System.Threading.Tasks;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Service;

namespace Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver
{
    public interface IActiveRaceResolver
    {
        Task<IActiveRaceControlService?> StartRace(int raceId);
        IActiveRaceControlService this[int raceId] { get; }
        Task<bool> EndRace(int raceId);
    }
}