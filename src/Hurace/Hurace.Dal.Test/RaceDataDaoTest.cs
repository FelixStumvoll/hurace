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
    public class RaceDataDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => 
            Assert.AreEqual(34, (await RaceDataDao.FindAllAsync()).Count());

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
            raceData.EventTypeId = (int) Domain.Enums.RaceDataEvent.RaceCanceled;
            await RaceDataDao.UpdateAsync(raceData);
            Assert.AreEqual((int)Domain.Enums.RaceDataEvent.RaceCanceled,(await RaceDataDao.FindByIdAsync(raceData.Id))?.EventTypeId);
        }

        [Test]
        public async Task InsertTest()
        {
            var raceId = (await RaceDao.FindAllAsync()).First().Id;
            var raceDataId = await RaceDataDao.InsertGetIdAsync(new RaceData
            {
                RaceId = raceId,
                EventDateTime = new DateTime(1969,4,20),
                EventTypeId = (int) Domain.Enums.RaceDataEvent.RaceFinished
            });

            var raceDataById = await RaceDataDao.FindByIdAsync(raceDataId.Value);
            Assert.AreEqual(raceId, raceDataById.RaceId);
            Assert.AreEqual(new DateTime(1969,4,20),raceDataById.EventDateTime );
            Assert.AreEqual((int)Domain.Enums.RaceDataEvent.RaceFinished, raceDataById.EventTypeId);
        }
        
        [Test]
        public async Task DeleteTest()
        {
            var raceDataId = await RaceDataDao.InsertGetIdAsync(new RaceData
            {
                RaceId = (await RaceDao.FindAllAsync()).First().Id,
                EventDateTime = DateTime.Now,
                EventTypeId = (await EventTypeDao.FindAllAsync()).First().Id
            });
            await RaceDataDao.DeleteAsync(raceDataId.Value);
            Assert.IsNull(await RaceDataDao.FindByIdAsync(raceDataId.Value));
        }

        [Test]
        public void DeleteAllTest() => Assert.ThrowsAsync<SqlException>(async () => await RaceDataDao.DeleteAllAsync());
    }
}