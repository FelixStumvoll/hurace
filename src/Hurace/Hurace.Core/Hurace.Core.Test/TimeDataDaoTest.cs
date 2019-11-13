using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class TimeDataDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupTimeData();

        [Test]
        public async Task FindAllTest()
        {
            var timeData = (await TimeDataDao.FindAllAsync()).ToList();
            Assert.AreEqual(30, timeData.Count());
            Assert.AreEqual(DateTime.Today, timeData.First().Time.Date);
        }

        [Test]
        public async Task FindByIdTest()
        {
            var skier = (await SkierDao.FindAllAsync()).First();
            var race = (await RaceDao.FindAllAsync()).First();
            var sensor = (await SensorDao.FindAllAsync()).First();
            var timeData = await TimeDataDao.FindByIdAsync(skier.Id, race.Id, sensor.Id);
            Assert.NotNull(timeData);
            Assert.NotNull(timeData.StartList);
            Assert.NotNull(timeData.SkierEvent);
            Assert.NotNull(timeData.SkierEvent.RaceData);
            Assert.NotNull(timeData.Sensor);
            Assert.AreEqual(DateTime.Today, timeData.Time.Date);
        }

        [Test]
        public async Task UpdateTest()
        {
            var skier = (await SkierDao.FindAllAsync()).First();
            var race = (await RaceDao.FindAllAsync()).First();
            var sensor = (await SensorDao.FindAllAsync()).First();
            var timeData = await TimeDataDao.FindByIdAsync(skier.Id, race.Id, sensor.Id);
            timeData.Time = new DateTime(2018, 11, 6);
            await TimeDataDao.UpdateAsync(timeData);
            timeData = await TimeDataDao.FindByIdAsync(skier.Id, race.Id, sensor.Id);
            Assert.AreEqual(new DateTime(2018, 11, 6), timeData.Time);
        }

        [Test]
        public async Task InsertTest()
        {
            var skier = (await SkierDao.FindAllAsync()).First();
            var race = (await RaceDao.FindAllAsync()).First();
            var skierEventId = (await SkierEventDao.FindAllAsync()).First().Id;
            var newSensorId =
                await SensorDao.InsertGetIdAsync(new Sensor {RaceId = race.Id, SensorDescription = "Description"});
            await TimeDataDao.InsertAsync(new TimeData
            {
                SkierEventId = skierEventId,
                RaceId = race.Id,
                SensorId = newSensorId,
                Time = new DateTime(2019, 11, 6),
                SkierId = skier.Id
            });
            
            var timeData = await TimeDataDao.FindByIdAsync(skier.Id, race.Id, newSensorId);
            
            Assert.NotNull(timeData);
            Assert.AreEqual(new DateTime(2019,11,6), timeData.Time);
        }

        [Test]
        public async Task DeleteTest()
        {
            var timeData = (await TimeDataDao.FindAllAsync()).First();
            await TimeDataDao.DeleteAsync(timeData.SkierId, timeData.RaceId, timeData.SensorId);
            Assert.IsNull(await TimeDataDao.FindByIdAsync(timeData.SkierId, timeData.RaceId, timeData.SensorId));
        }
        
        [Test]
        public async Task DeleteAllTest()
        {
            await TimeDataDao.DeleteAllAsync();
            Assert.AreEqual(0, (await TimeDataDao.FindAllAsync()).Count());
        }

        [Test]
        public async Task GetRaceForRankingTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            var res = (await TimeDataDao.GetRankingForRace(race.Id)).ToList();
            Assert.AreEqual(5, res.Count());
            Assert.IsTrue(res[0].RaceTime < res[1].RaceTime);
        }
    }
}