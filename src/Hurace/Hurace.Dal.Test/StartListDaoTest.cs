﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class StartListDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupStartList();

        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await StartListDao.FindAllAsync()).Count());

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
            var skierId = await SetupSkier();
            var raceId = (await RaceDao.FindAllAsync()).First().Id;
            await StartListDao.InsertAsync(new StartList
            {
                SkierId = skierId,
                RaceId = raceId,
                StartNumber = 99,
                StartStateId = (int) Constants.RaceState.Finished
            });
            var startList = await StartListDao.FindByIdAsync(skierId, raceId);
            
            Assert.AreEqual(skierId, startList.SkierId);
            Assert.AreEqual(raceId, startList.RaceId);
            Assert.AreEqual((int) Constants.RaceState.Finished, startList.StartStateId);
            Assert.NotNull(startList.Skier);
            Assert.NotNull(startList.StartState);
            Assert.NotNull(startList.Race);
        }
        
        [Test]
        public async Task DeleteTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            await StartListDao.DeleteAsync(startList.RaceId, startList.SkierId);
            Assert.IsNull(await StartListDao.FindByIdAsync(startList.SkierId, startList.RaceId));
        }
        
        [Test]
        public async Task DeleteAllTest()
        {
            await StartListDao.DeleteAllAsync();
            Assert.AreEqual(0, (await StartListDao.FindAllAsync()).Count());
        }
        
        [Test]
        public async Task GetStartListForRaceTest()
        {
            var raceId = (await RaceDao.FindAllAsync()).First().Id;
            Assert.AreEqual(1, (await StartListDao.GetStartListForRace(raceId)).Count());
        }
        
        [Test]
        public async Task GetCurrentSkierTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            startList.StartStateId = (int) Constants.StartState.Running;
            await StartListDao.UpdateAsync(startList);
            Assert.AreEqual(startList.SkierId, (await StartListDao.GetCurrentSkierForRace(startList.RaceId))?.SkierId);
        }
        
        [Test]
        public async Task GetNextSkierTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            startList.StartStateId = (int) Constants.StartState.Upcoming;
            await StartListDao.UpdateAsync(startList);
            Assert.AreEqual(startList.SkierId, (await StartListDao.GetNextSkierForRace(startList.RaceId))?.SkierId);
        }
    }
}