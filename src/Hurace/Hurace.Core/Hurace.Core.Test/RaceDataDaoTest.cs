using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class RaceDataDaoTest : TestBase
    {

        [SetUp]
        public Task BeforeEach() => SetupRaceData();

        [Test]
        public async Task FindAllTest() => 
            Assert.AreEqual(1, (await RaceDataDao.FindAllAsync()).Count());

        [Test]
        public async Task FindByIdTest()
        {
            var raceDataId = (await RaceDataDao.FindAllAsync()).First().Id;
            var raceData = await RaceDataDao.FindByIdAsync(raceDataId);
            Assert.NotNull(raceData);
            Assert.NotNull(raceData?.EventType);
        }
        
        [Test]
        public async Task UpdateTest()
        {
            var raceData = (await RaceDataDao.FindAllAsync()).First();
            raceData.EventTypeId = (int) Constants.RaceEvent.Canceled;
            await RaceDataDao.UpdateAsync(raceData);
            Assert.AreEqual((int)Constants.RaceEvent.Canceled,(await RaceDataDao.FindByIdAsync(raceData.Id))?.EventTypeId);
        }

        [Test]
        public async Task InsertTest()
        {
            var raceId = (await RaceDao.FindAllAsync()).First().Id;
            await RaceDataDao.InsertAsync(new RaceData
            {
                RaceId = raceId,
                EventDateTime = DateTime.Now,
                EventTypeId = (int) Constants.RaceEvent.Finished
            });
            
            Assert.AreEqual(2, (await RaceDataDao.FindAllAsync()).Count());
        }
        
        [Test]
        public async Task DeleteTest()
        {
            var raceDataId = (await RaceDataDao.FindAllAsync()).First().Id;
            await RaceDataDao.DeleteAsync(raceDataId);
            Assert.IsNull(await RaceDataDao.FindByIdAsync(raceDataId));
        }

        [Test]
        public async Task DeleteAllTest()
        {
            await RaceDataDao.DeleteAllAsync();
            Assert.AreEqual(0, (await RaceDataDao.FindAllAsync()).Count());
        }
    }
}