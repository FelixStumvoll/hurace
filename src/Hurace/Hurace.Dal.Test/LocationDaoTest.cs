using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class LocationDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupLocation();

        [Test]
        public async Task GetPossibleDisciplinesTest()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            Assert.AreEqual(1, (await LocationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
        }

        [Test]
        public async Task AddPossibleDisciplineTest()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            var disciplineId = await DisciplineDao.InsertGetIdAsync(new Discipline
            {
                DisciplineName = "XYZ"
            });
            await LocationDao.InsertPossibleDisciplineForLocation(location.Id, disciplineId.Value);
            Assert.AreEqual(2, (await LocationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
        }

        [Test]
        public async Task RemovePossibleDisciplineTest()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            var disciplineId = (await DisciplineDao.FindAllAsync()).First().Id;
            await LocationDao.DeletePossibleDisciplineForLocation(location.Id, disciplineId);
            Assert.AreEqual(0, (await LocationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
        }

        [Test]
        public async Task GetCountryFromLocation()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            Assert.AreEqual("AT", location.Country?.CountryCode);
        }

        [Test]
        public async Task DeleteAllTest()
        {
            await LocationDao.DeleteAllAsync();
            Assert.IsEmpty(await LocationDao.FindAllAsync());
        }

        [Test]
        public async Task DeleteTest()
        {
            var id = (await LocationDao.FindAllAsync()).First().Id;
            await LocationDao.DeleteAsync(id);
            Assert.AreEqual(2, (await LocationDao.FindAllAsync()).Count());
        }

        [Test]
        public async Task FindAllTest()
        {
            Assert.AreEqual(3, (await LocationDao.FindAllAsync()).Count());
        }

        [Test]
        public async Task FindByIdTest()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            Assert.AreEqual(location.LocationName, (await LocationDao.FindByIdAsync(location.Id))?.LocationName);
        }

        [Test]
        public async Task InsertTest()
        {
            var countryId = (await CountryDao.FindAllAsync()).First().Id;
            var locationId = await LocationDao.InsertGetIdAsync(new Location
            {
                CountryId = countryId,
                LocationName = "Name"
            });

            Assert.AreEqual(4, (await LocationDao.FindAllAsync()).Count());
            Assert.AreEqual("Name", (await LocationDao.FindByIdAsync(locationId.Value))?.LocationName);
            Assert.NotNull((await LocationDao.FindByIdAsync(locationId.Value))?.Country);
        }

        [Test]
        public async Task UpdateTest()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            location.LocationName = "Test123";
            await LocationDao.UpdateAsync(location);
            Assert.AreEqual(location.LocationName, (await LocationDao.FindByIdAsync(location.Id))?.LocationName);
        }
    }
}