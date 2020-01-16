using System.Threading.Tasks;

namespace Hurace.Core.Interface
{
    public interface IActiveRaceResolver
    {
        Task<IActiveRaceControlService?> StartRace(int raceId);
        IActiveRaceControlService this[int raceId] { get; }
    }
}