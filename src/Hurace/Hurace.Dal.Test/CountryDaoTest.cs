using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class CountryDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest()
        {
            var countries = await CountryDao.FindAllAsync();
            Assert.AreEqual(12, countries.Count());
        }

        [Test]
        public async Task InsertTest()
        {
            var id = await CountryDao.InsertGetIdAsync(new Country
            {
                CountryCode = "XY",
                CountryName = "TestCountry"
            });

            var country = await CountryDao.FindByIdAsync(id.Value);
            Assert.AreEqual("XY", country?.CountryCode);
            Assert.AreEqual("TestCountry", country?.CountryName ?? "");
        }
        
        [Test]
        public async Task UpdateTest()
        {
            var country = (await CountryDao.FindAllAsync()).First();
            country.CountryName = "TestABC";
            await CountryDao.UpdateAsync(country);
            var updatedCountry = (await CountryDao.FindByIdAsync(country.Id));
            Assert.AreEqual(country.CountryName, updatedCountry?.CountryName);
        }

        [Test]
        public async Task FindByIdTest()
        {
            var country = (await CountryDao.FindAllAsync()).First();
            var countryById = await CountryDao.FindByIdAsync(country.Id);
            Assert.AreEqual(country.CountryCode, countryById?.CountryCode);
            Assert.AreEqual(country.CountryName, countryById?.CountryName);
        }

        [Test]
        public async Task DeleteTest()
        {
            var countryId = await CountryDao.InsertGetIdAsync(new Country {CountryName = "ABC", CountryCode = "DF"});
            await CountryDao.DeleteAsync(countryId.Value);
            Assert.IsNull(await CountryDao.FindByIdAsync(countryId.Value));
        }

        [Test]
        public async Task DeleteAllTest()
        {
            await DbTeardown();
            var countryId = await CountryDao.InsertGetIdAsync(new Country {CountryName = "ABC", CountryCode = "DF"});
            
            await CountryDao.DeleteAllAsync();
            Assert.IsEmpty(await CountryDao.FindAllAsync());
        }
    }
}