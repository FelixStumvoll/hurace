using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class SeasonDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupSeason();
        
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await SeasonDao.FindAllAsync()).Count());
        
        [Test]
        public async Task FindByIdTest()
        {
            var id = (await SeasonDao.FindAllAsync()).First().Id;
            Assert.NotNull(await SeasonDao.FindByIdAsync(id));
        }
        
        [Test]
        public async Task UpdateTest()
        {
            var season = (await SeasonDao.FindAllAsync()).First();
            season.StartDate = season.StartDate.AddDays(-5);
            await SeasonDao.UpdateAsync(season);
            Assert.AreEqual(season.StartDate, (await SeasonDao.FindByIdAsync(season.Id))?.StartDate);
        }
        
        [Test]
        public async Task InsertTest()
        {
            await SeasonDao.InsertAsync(new Season
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            });
            
            Assert.AreEqual(2, (await SeasonDao.FindAllAsync()).Count());
        }
        
        [Test]
        public async Task DeleteTest()
        {
            var id = (await SeasonDao.FindAllAsync()).First().Id;
            await SeasonDao.DeleteAsync(id);
            Assert.IsNull(await SeasonDao.FindByIdAsync(id));
        }
        
        [Test]
        public async Task DeleteAllTest()
        {
            await SeasonDao.DeleteAllAsync();
            Assert.AreEqual(0, (await SeasonDao.FindAllAsync()).Count());
        }
    }
}