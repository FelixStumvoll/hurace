using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class SkierEventDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupSkierEvent();

        [Test]
        public async Task Test() => Assert.AreEqual(1, (await SkierEventDao.FindAllAsync()).Count());

        [Test]
        public async Task FindByIdTest()
        {
            var skierEvent = (await SkierEventDao.FindAllAsync()).First();
            var skierEventById = await SkierEventDao.FindByIdAsync(skierEvent.Id);
            Assert.AreEqual(skierEvent.RaceData.EventTypeId, skierEventById.RaceData.EventTypeId);
            Assert.NotNull(skierEventById.StartList);
        }

        [Test]
        public async Task UpdateTest()
        {
            var skierEvent = (await SkierEventDao.FindAllAsync()).First();
            var skierId = await InsertSkier((await CountryDao.FindAllAsync()).First().Id);
            await StartListDao.InsertAsync(new StartList
            {
                RaceId = skierEvent.RaceData.RaceId, SkierId = skierId, StartNumber = 50,
                StartStateId = (int) Constants.StartState.Finished
            });
            skierEvent.SkierId = skierId;
            await SkierEventDao.UpdateAsync(skierEvent);
            Assert.AreEqual(skierEvent.SkierId, (await SkierEventDao.FindByIdAsync(skierEvent.Id)).SkierId);
        }
    }
}