using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;
using Gender = Hurace.Dal.Domain.Enums.Gender;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class StartListDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(10, (await StartListDao.FindAllAsync()).Count());

        [Test]
        public async Task FindByIdTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            var startListById = await StartListDao.FindByIdAsync(startList.SkierId, startList.RaceId);
            Assert.NotNull(startListById);
            Assert.NotNull(startListById.Race);
            Assert.NotNull(startListById.Skier);
            Assert.NotNull(startListById.Skier?.Country);
            Assert.NotNull(startListById.Skier?.Gender);
            Assert.NotNull(startListById.StartState);
        }

        [Test]
        public async Task UpdateTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            startList.StartNumber = 999;
            await StartListDao.UpdateAsync(startList);
            Assert.AreEqual(startList.StartNumber,
                            (await StartListDao.FindByIdAsync(startList.SkierId, startList.RaceId))?.StartNumber);
        }

        [Test]
        public async Task InsertTest()
        {
            var skierId = (await SkierDao.FindAllAsync()).First().Id;
            var raceId = (await RaceDao.FindAllAsync()).First().Id;

            var newSkierId = await SkierDao.InsertGetIdAsync(new Skier
            {
                Retired = false,
                CountryId = (await CountryDao.FindAllAsync()).First().Id,
                FirstName = "ABC",
                LastName = "DEF",
                GenderId = 1,
                DateOfBirth = DateTime.Now
            });

            await StartListDao.InsertAsync(new StartList
            {
                SkierId = newSkierId.Value,
                RaceId = raceId,
                StartNumber = 99,
                StartStateId = (int) Domain.Enums.RaceState.Finished
            });
            var startList = await StartListDao.FindByIdAsync(skierId, raceId);

            Assert.AreEqual(skierId, startList.SkierId);
            Assert.AreEqual(raceId, startList.RaceId);
            Assert.AreEqual((int) Domain.Enums.RaceState.Finished, startList.StartStateId);
            Assert.NotNull(startList.Skier);
            Assert.NotNull(startList.StartState);
            Assert.NotNull(startList.Race);
        }

        // [Test]
        // public async Task DeleteTest()
        // {
        //     var startList = (await StartListDao.FindAllAsync()).First();
        //     await StartListDao.DeleteAsync(startList.RaceId, startList.SkierId);
        //     Assert.IsNull(await StartListDao.FindByIdAsync(startList.SkierId, startList.RaceId));
        // }
        //
        // [Test]
        // public async Task DeleteAllTest()
        // {
        //     await StartListDao.DeleteAllAsync();
        //     Assert.AreEqual(0, (await StartListDao.FindAllAsync()).Count());
        // }

        [Test]
        public async Task DeleteAllForRaceTest()
        {
        }

        [Test]
        public async Task GetDisqualifiedSkierForRaceTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            startList.StartStateId = 4;
            await StartListDao.UpdateAsync(startList);

            Assert.IsTrue(
                (await StartListDao.GetDisqualifiedSkierForRace(startList.RaceId)).Any(
                    s => s.SkierId == startList.SkierId));
        }

        [Test]
        public async Task GetStartListForRaceTest()
        {
            var raceId = (await RaceDao.FindAllAsync()).First().Id;
            Assert.AreEqual(5, (await StartListDao.GetStartListForRace(raceId)).Count());
        }
        
        [Test]
        public async Task CountStartListForRaceTest()
        {
            var raceId = (await RaceDao.FindAllAsync()).First().Id;
            Assert.AreEqual(5, (await StartListDao.CountStartListForRace(raceId)));
        }
        
        [Test]
        public async Task GetSkierForRaceTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            Assert.AreEqual(startList.SkierId, (await StartListDao.FindByIdAsync(startList.SkierId, startList.RaceId)).SkierId);
        }

        [Test]
        public async Task GetCurrentSkierTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            startList.StartStateId = (int) Domain.Enums.StartState.Running;
            await StartListDao.UpdateAsync(startList);
            Assert.AreEqual(startList.SkierId, (await StartListDao.GetCurrentSkierForRace(startList.RaceId))?.SkierId);
        }

        [Test]
        public async Task GetNextSkierTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            startList.StartStateId = (int) Domain.Enums.StartState.Upcoming;
            await StartListDao.UpdateAsync(startList);
            Assert.AreEqual(startList.SkierId, (await StartListDao.GetNextSkierForRace(startList.RaceId))?.SkierId);
        }

        [Test]
        public async Task GetRemainingStartListForRaceTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            
            Assert.AreEqual(0, (await StartListDao.GetRemainingStartListForRace(race.Id)).Count());

            var skierId = await SkierDao.InsertGetIdAsync(new Skier
            {
                Retired = false,
                FirstName = "F",
                LastName = "S",
                CountryId = (await CountryDao.FindAllAsync()).First().Id,
                GenderId = race.GenderId,
                DateOfBirth = DateTime.Now
            });

            await StartListDao.InsertAsync(new StartList
            {
                RaceId = race.Id, SkierId = skierId.Value, StartNumber = 99, StartStateId = (int) StartState.Upcoming
            });

            Assert.AreEqual(1, (await StartListDao.GetRemainingStartListForRace(race.Id)).Count());

        }
    }
}