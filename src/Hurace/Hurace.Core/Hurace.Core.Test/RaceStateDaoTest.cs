using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class RaceStateDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(4, (await RaceStateDao.FindAllAsync()).Count());

        [Test]
        public async Task FindById() =>
            Assert.AreEqual("Finished", (await RaceStateDao.FindByIdAsync(3))?.RaceStateDescription);
    }
}