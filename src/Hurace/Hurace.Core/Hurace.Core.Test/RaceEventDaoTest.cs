using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    public class RaceEventDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupRaceEvent();
        
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await RaceEventDao.FindAllAsync()).Count());
        
        [Test]
        public async Task FindByIdTest()
        {
            var raceEvent =(await RaceEventDao.FindAllAsync()).First();
            raceEvent = (await RaceEventDao.FindByIdAsync(raceEvent.Id));
            Assert.NotNull(raceEvent);
            Assert.NotNull(raceEvent.RaceData);
        }
    }
}