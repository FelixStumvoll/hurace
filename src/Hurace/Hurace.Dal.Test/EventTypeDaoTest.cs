using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class EventTypeDaoTest : TestBase
    {
        [Test]
        public async Task FindAllAsync() => Assert.AreEqual(9, (await EventTypeDao.FindAllAsync()).Count());

        [Test]
        public async Task FindByIf() =>
            Assert.AreEqual("Skier Disqualified", (await EventTypeDao.FindByIdAsync(6))?.EventDescription);
    }
}