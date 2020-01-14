using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class TimeDataDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest()
        {
            var timeData = (await TimeDataDao.FindAllAsync()).ToList();
            Assert.AreEqual(10, timeData.Count());
            Assert.AreEqual(DateTime.Today.Millisecond, timeData.First().Time);
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
            Assert.NotNull(timeData?.SkierEvent);
            Assert.NotNull(timeData?.SkierEvent?.RaceData);
            Assert.NotNull(timeData?.Sensor);
            Assert.AreEqual(DateTime.Today.Millisecond, timeData?.Time);
        }

        [Test]
        public async Task UpdateTest()
        {
            var skier = (await SkierDao.FindAllAsync()).First();
            var race = (await RaceDao.FindAllAsync()).First();
            var sensor = (await SensorDao.FindAllAsync()).First();
            var timeData = await TimeDataDao.FindByIdAsync(skier.Id, race.Id, sensor.Id);
            if (timeData != null)
            {
                timeData.Time = 500;
                await TimeDataDao.UpdateAsync(timeData);
                timeData = await TimeDataDao.FindByIdAsync(skier.Id, race.Id, sensor.Id);
                Assert.AreEqual(500, timeData?.Time);
            }
            else Assert.Fail("TimeData was null");
        }

        [Test]
        public async Task InsertTest()
        {
            var skier = (await SkierDao.FindAllAsync()).First();
            var race = (await RaceDao.FindAllAsync()).First();
            var skierEventId = (await SkierEventDao.FindAllAsync()).First().Id;
            var newSensorId =
                await SensorDao.InsertGetIdAsync(new Sensor {RaceId = race.Id, SensorNumber = 0});
            await TimeDataDao.InsertAsync(new TimeData
            {
                SkierEventId = skierEventId,
                RaceId = race.Id,
                SensorId = newSensorId.Value,
                Time = new DateTime(2019, 11, 6).Millisecond,
                SkierId = skier.Id
            });

            var timeData = await TimeDataDao.FindByIdAsync(skier.Id, race.Id, newSensorId.Value);

            Assert.NotNull(timeData);
            Assert.AreEqual(new DateTime(2019, 11, 6).Millisecond, timeData?.Time);
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
        public async Task GetRankingForSensorTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            var ranking = (await TimeDataDao.GetRankingForSensor(race.Id, 1)).ToList();
            Assert.AreEqual(5, ranking.Count());
            Assert.IsTrue(ranking[0].Time < ranking[1].Time);
        }

        [Test]
        public async Task GetTimeDataForStartListTest()
        {
            var startList = (await StartListDao.FindAllAsync()).First();
            Assert.AreEqual(
                2, (await TimeDataDao.GetTimeDataForStartList(startList.SkierId, startList.RaceId)).Count());
        }

        [Test]
        public async Task CountTimeDataForRaceTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            Assert.AreEqual(10, await TimeDataDao.CountTimeDataForRace(race.Id));
        }

        [Test]
        public async Task GetAverageTimeForSensorTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            var timeDatas = (await TimeDataDao.FindAllAsync());
            var tasks = timeDatas.Where(t => t.RaceId == race.Id && t.Sensor.SensorNumber == 1).Select(
                     async t =>
                     {
                         t.Time = 1500;
                         await TimeDataDao.UpdateAsync(t);
                     });
            await Task.WhenAll(tasks);
            Assert.AreEqual(1500, await TimeDataDao.GetAverageTimeForSensor(race.Id, 1));
        }

        [Test]
        public async Task GetStartTimeForStartListTest()
        {
            var skierEvents = (await SkierEventDao.FindAllAsync());

            var startEvent = skierEvents.First(sk => sk.RaceData.EventTypeId == 8);
            startEvent.RaceData.EventDateTime = DateTime.Today;
            await RaceDataDao.UpdateAsync(startEvent.RaceData);
            
            Assert.AreEqual(DateTime.Today,
                            (await TimeDataDao.GetStartTimeForStartList(startEvent.SkierId, startEvent.RaceId)).Value);
        }
        
        // [Test]
        // public async Task GetRaceForRankingTest()
        // {
        //     var race = (await RaceDao.FindAllAsync()).First();
        //     var res = (await TimeDataDao.GetRankingForRace(race.Id)).ToList();
        //     Assert.AreEqual(5, res.Count);
        //     Assert.IsTrue(res[0].RaceTime < res[1].RaceTime);
        // }
    }
}