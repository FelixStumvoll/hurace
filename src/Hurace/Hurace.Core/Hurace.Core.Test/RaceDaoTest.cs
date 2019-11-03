using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class RaceDaoTest : TestBase
    {

        [SetUp]
        public async Task BeforeEach() => await SetupRace();
        
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await RaceDao.FindAllAsync()).Count());

        [Test]
        public async Task FindByIdTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            Assert.AreEqual(race.RaceDescription, (await RaceDao.FindByIdAsync(race.Id)).RaceDescription);
        }

        [Test]
        public async Task InsertTest()
        {
            var disciplineId = (await DisciplineDao.FindAllAsync()).First().Id;
            var locationId = (await LocationDao.FindAllAsync()).First().Id;
            var seasonId = (await SeasonDao.FindAllAsync()).First().Id;
            
            await RaceDao.InsertAsync(new Race
            {
                DisciplineId = disciplineId,
                GenderId = 1,
                LocationId = locationId,
                RaceDescription = "Description",
                SeasonId = seasonId,
                RaceStateId = (int) Constants.RaceEvent.Finished,
                RaceDate = DateTime.Now
            });

            Assert.AreEqual(2, (await RaceDao.FindAllAsync()).Count());
        }

        [Test]
        public async Task UpdateTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            race.RaceDescription = "Test123";
            await RaceDao.UpdateAsync(race);
            Assert.AreEqual(race.RaceDescription, (await RaceDao.FindByIdAsync(race.Id)).RaceDescription);
        }

        [Test]
        public async Task DeleteTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            await RaceDao.DeleteAsync(race.Id);
            Assert.IsNull(await RaceDao.FindByIdAsync(race.Id));
        }
        
        [Test]
        public async Task DeleteAllTest()
        {
            await RaceDao.DeleteAllAsync();
            Assert.AreEqual(0, (await RaceDao.FindAllAsync()).Count());
        }
    }
}