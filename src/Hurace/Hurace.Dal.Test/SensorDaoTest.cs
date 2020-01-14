using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class SensorDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(2, (await SensorDao.FindAllAsync()).Count());

        [Test]
        public async Task FindByIdTest()
        {
            var id = (await SensorDao.FindAllAsync()).First().Id;
            Assert.NotNull(await SensorDao.FindByIdAsync(id));
        }

        [Test]
        public async Task UpdateTest()
        {
            var sensor = (await SensorDao.FindAllAsync()).First();
            sensor.SensorNumber = 1;
            await SensorDao.UpdateAsync(sensor);
            Assert.AreEqual(sensor.SensorNumber, (await SensorDao.FindByIdAsync(sensor.Id))?.SensorNumber);
        }

        [Test]
        public async Task InsertTest()
        {
            var raceId = (await RaceDao.FindAllAsync()).First().Id;
            var id = await SensorDao.InsertGetIdAsync(new Sensor
            {
                RaceId = raceId,
                SensorNumber = 21
            });
            var sensor = await SensorDao.FindByIdAsync(id.Value);
            Assert.AreEqual(21, sensor.SensorNumber);
            Assert.AreEqual(raceId, sensor.RaceId);
        }

        [Test]
        public async Task FindAllSensorsForRaceTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            Assert.AreEqual(2, (await SensorDao.FindAllSensorsForRace(race.Id)).Count());
        }
        
        [Test]
        public async Task GetLastSensorNumberTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            Assert.AreEqual(1, (await SensorDao.GetLastSensorNumber(race.Id)));
        }
        
        [Test]
        public async Task GetSensorForSensorNumberTest()
        {
            var race = (await RaceDao.FindAllAsync()).First();
            var sensor = (await SensorDao.FindAllSensorsForRace(race.Id)).First();
            Assert.AreEqual(sensor.Id, (await SensorDao.GetSensorForSensorNumber(sensor.SensorNumber, race.Id)).Id);
        }
        // [Test]
        // public async Task DeleteTest()
        // {
        //     var id = (await SensorDao.FindAllAsync()).First().Id;
        //     await SensorDao.DeleteAsync(id);
        //     Assert.IsNull(await SensorDao.FindByIdAsync(id));
        // }
        //
        // [Test]
        // public async Task DeleteAllTest()
        // {
        //     await SensorDao.DeleteAllAsync();
        //     Assert.AreEqual(0, (await SensorDao.FindAllAsync()).Count());
        // }
    }
}