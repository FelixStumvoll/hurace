using System.Threading.Tasks;
using Hurace.Core.Timer;

namespace Hurace.Core.Interface
{
    public interface IRaceClockProvider
    {
        Task<IRaceClock?> GetRaceClock();
    }
}