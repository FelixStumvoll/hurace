using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class SkierEventDaoTest : TestBase
    {
        [Test]
        public async Task Test() => Assert.AreEqual(30, (await SkierEventDao.FindAllAsync()).Count());

        [Test]
        public async Task FindByIdTest()
        {
            var skierEvent = (await SkierEventDao.FindAllAsync()).First();
            var skierEventById = await SkierEventDao.FindByIdAsync(skierEvent.Id);
            Assert.AreEqual(skierEvent.RaceData?.EventTypeId, skierEventById?.RaceData?.EventTypeId);
            Assert.NotNull(skierEventById?.StartList);
        }

        [Test]
        public async Task UpdateTest()
        {
            var skierEvent = (await SkierEventDao.FindAllAsync()).First();
            var skierId = (await SkierDao.FindAllAsync()).First().Id;
            skierEvent.SkierId = skierId;
            await SkierEventDao.UpdateAsync(skierEvent);
            Assert.AreEqual(skierEvent.SkierId, (await SkierEventDao.FindByIdAsync(skierEvent.Id))?.SkierId);
        }
    }
}