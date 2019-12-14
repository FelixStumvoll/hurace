using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class RaceDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupRace();

        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await RaceDao.FindAllAsync()).Count());

        [Test]
        public async Task FindByIdTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            var raceById = await RaceDao.FindByIdAsync(race.Id);
            Assert.AreEqual(race.RaceDescription, raceById?.RaceDescription);
            Assert.NotNull(raceById?.Gender);
            Assert.NotNull(raceById?.Location);
            Assert.NotNull(raceById?.Season);
            Assert.NotNull(raceById?.RaceState);
        }

        [Test]
        public async Task InsertTest()
        {
            var disciplineId = (await DisciplineDao.FindAllAsync()).First().Id;
            var locationId = (await LocationDao.FindAllAsync()).First().Id;
            var seasonId = (await SeasonDao.FindAllAsync()).First().Id;

            var raceId = await RaceDao.InsertGetIdAsync(new Race
            {
                DisciplineId = disciplineId,
                GenderId = (int) Constants.Gender.Male,
                LocationId = locationId,
                RaceDescription = "Description",
                SeasonId = seasonId,
                RaceStateId = (int) Constants.RaceEvent.Finished,
                RaceDate = new DateTime(2019, 11, 15)
            });

            var raceById = await RaceDao.FindByIdAsync(raceId.Value);
            Assert.AreEqual(disciplineId, raceById.DisciplineId);
            Assert.AreEqual((int) Constants.Gender.Male, raceById.GenderId);
            Assert.AreEqual(locationId, raceById.LocationId);
            Assert.AreEqual("Description", raceById.RaceDescription);
            Assert.AreEqual(seasonId, raceById.SeasonId);
            Assert.AreEqual((int) Constants.RaceEvent.Finished, raceById.RaceStateId);
            Assert.AreEqual(new DateTime(2019, 11, 15), raceById.RaceDate);
            Assert.NotNull(raceById.Location);
            Assert.NotNull(raceById.Gender);
            Assert.NotNull(raceById.Season);
        }

        [Test]
        public async Task UpdateTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            race.RaceDescription = "Test123";
            await RaceDao.UpdateAsync(race);
            Assert.AreEqual(race.RaceDescription, (await RaceDao.FindByIdAsync(race.Id))?.RaceDescription);
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