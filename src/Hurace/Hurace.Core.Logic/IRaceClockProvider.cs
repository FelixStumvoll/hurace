using System.Threading.Tasks;
using Hurace.Core.Timer;

namespace Hurace.Core.Logic
{
    public interface IRaceClockProvider
    {
        Task<IRaceClock?> GetRaceClock();
    }
}