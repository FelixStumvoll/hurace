using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class SensorDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupSensor();

        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await SensorDao.FindAllAsync()).Count());

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
            sensor.SensorDescription = "Test123";
            await SensorDao.UpdateAsync(sensor);
            Assert.AreEqual(sensor.SensorDescription, (await SensorDao.FindByIdAsync(sensor.Id))?.SensorDescription);
        }

        [Test]
        public async Task InsertTest()
        {
            var raceId = (await RaceDao.FindAllAsync()).First().Id;
            var id = await SensorDao.InsertGetIdAsync(new Sensor
            {
                RaceId = raceId,
                SensorDescription = "Description123"
            });
            var sensor = await SensorDao.FindByIdAsync(id);
            Assert.AreEqual("Description123", sensor.SensorDescription);
            Assert.AreEqual(raceId, sensor.RaceId);
        }
        
        [Test]
        public async Task DeleteTest()
        {
            var id = (await SensorDao.FindAllAsync()).First().Id;
            await SensorDao.DeleteAsync(id);
            Assert.IsNull(await SensorDao.FindByIdAsync(id));
        }

        [Test]
        public async Task DeleteAllTest()
        {
            await SensorDao.DeleteAllAsync();
            Assert.AreEqual(0, (await SensorDao.FindAllAsync()).Count());
        }
    }
}