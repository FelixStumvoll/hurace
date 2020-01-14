using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Configs;
using Hurace.Core.Simulation;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    public class RaceClockProviderTest
    {
        [ExcludeFromCodeCoverage]
        public class FailureClock
        {
            
        }
        
        [Test]
        public async Task GetRaceClockTest()
        {
            var clockConfig = new ClockConfig("Hurace.Core.Simulation", "MockRaceClockV2");
            var clockProvider = new RaceClockProvider(clockConfig);
            var clock = await clockProvider.GetRaceClock();
            Assert.That(clock is MockRaceClockV2);
        }
        
        [Test]
        public async Task GetRaceClockFailureTest()
        {
            var clockConfig = new ClockConfig("Hurace.Core.Test", "FailureClock");
            var clockProvider = new RaceClockProvider(clockConfig);
            var clock = await clockProvider.GetRaceClock();
            Assert.IsNull(clock);
        }
    }
}