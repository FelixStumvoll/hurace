using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class LocationDaoTest : TestBase
    {
        [Test]
        public async Task GetPossibleDisciplinesTest()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            Assert.AreEqual(2, (await LocationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
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
            Assert.AreEqual(3, (await LocationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
        }

        [Test]
        public async Task RemovePossibleDisciplineTest()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            var disciplineId = (await DisciplineDao.FindAllAsync()).First().Id;
            await LocationDao.DeletePossibleDisciplineForLocation(location.Id, disciplineId);
            Assert.AreEqual(1, (await LocationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
        }

        [Test]
        public async Task GetCountryFromLocation()
        {
            var location = (await LocationDao.FindAllAsync()).First();
            Assert.AreEqual("AUT", location.Country?.CountryCode);
        }

        // [Test]
        // public void DeleteAllTest()
        // {
        //     Assert.ThrowsAsync<SqlException>(async () => await LocationDao.Del());
        // }
        //
        // [Test]
        // public async Task DeleteTest()
        // {
        //     var id = await LocationDao.InsertGetIdAsync(new Location
        //     {
        //         CountryId = (await CountryDao.FindAllAsync()).First().Id,
        //         LocationName = "AAA"
        //     });
        //     await LocationDao.DeleteAsync(id.Value);
        //     Assert.IsNull(await LocationDao.FindByIdAsync(id.Value));
        // }

        [Test]
        public async Task FindAllTest()
        {
            Assert.AreEqual(1, (await LocationDao.FindAllAsync()).Count());
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