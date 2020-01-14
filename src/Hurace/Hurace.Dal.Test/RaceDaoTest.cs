using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class RaceDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(2, (await RaceDao.FindAllAsync()).Count());

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
                GenderId = (int) Domain.Enums.Gender.Male,
                LocationId = locationId,
                RaceDescription = "Description",
                SeasonId = seasonId,
                RaceStateId = (int)Domain.Enums.RaceDataEvent.RaceFinished,
                RaceDate = new DateTime(2019, 11, 15)
            });

            var raceById = await RaceDao.FindByIdAsync(raceId.Value);
            Assert.AreEqual(disciplineId, raceById.DisciplineId);
            Assert.AreEqual((int) Domain.Enums.Gender.Male, raceById.GenderId);
            Assert.AreEqual(locationId, raceById.LocationId);
            Assert.AreEqual("Description", raceById.RaceDescription);
            Assert.AreEqual(seasonId, raceById.SeasonId);
            Assert.AreEqual((int) Domain.Enums.RaceDataEvent.RaceFinished, raceById.RaceStateId);
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
        public async Task GetActiveRacesTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            race.RaceStateId = 2;
            await RaceDao.UpdateAsync(race);
            
            Assert.AreEqual(1, (await RaceDao.GetActiveRaces()).Count());
        }
        
        [Test]
        public async Task GetActiveRaceByIdTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            race.RaceStateId = 2;
            await RaceDao.UpdateAsync(race);
            
            Assert.NotNull(await RaceDao.GetActiveRaceById(race.Id));
        }
        
        [Test]
        public async Task GetRacesForSeason()
        {
            var season = (await SeasonDao.FindAllAsync()).First();

            Assert.AreEqual(2, (await RaceDao.GetRacesForSeasonId(season.Id)).Count());
        }

        // [Test]
        // public async Task DeleteTest()
        // {
        //     var race = (await RaceDao.FindAllAsync()).First();
        //     Assert.Throws<SqlException>(async () => await RaceDao.DeleteAsync(race.Id));
        // }
        //
        // [Test]
        // public async Task DeleteAllTest()
        // {
        //     Assert.Throws<SqlException>(async () => await RaceDao.DeleteAllAsync());
        // }
    }
}