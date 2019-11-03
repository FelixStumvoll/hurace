using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class CountryDaoTest : TestBase
    {
        [SetUp]
        public async Task BeforeEach() => await SetupCountry();

        [Test]
        public async Task FindAllTest()
        {
            var countries = await CountryDao.FindAllAsync();
            Assert.AreEqual(3, countries.Count());
        }

        [Test]
        public async Task InsertTest()
        {
            await CountryDao.InsertAsync(new Country
            {
                CountryCode = "XY",
                CountryName = "TestCountry"
            });

            var countries = await CountryDao.FindAllAsync();
            Assert.AreEqual(1, countries.Count(c => c.CountryCode.Equals("XY")));
        }
        
        [Test]
        public async Task UpdateTest()
        {
            var country = (await CountryDao.FindAllAsync()).First();
            await CountryDao.UpdateAsync(new Country
            {
                Id = country.Id,
                CountryCode = "XX",
                CountryName = "Abcd"
            });
            var updatedCountry = (await CountryDao.FindByIdAsync(country.Id));
            Assert.AreNotEqual(country.CountryName, updatedCountry.CountryName);
        }

        [Test]
        public async Task FindByIdTest()
        {
            var countries = (await CountryDao.FindAllAsync()).ToList();
            var country = await CountryDao.FindByIdAsync(countries.First().Id);
            Assert.AreEqual(countries.First().Id, country.Id);
        }

        [Test]
        public async Task DeleteTest()
        {
            var country = (await CountryDao.FindAllAsync()).First();
            await CountryDao.DeleteAsync(country.Id);
            Assert.IsNull(await CountryDao.FindByIdAsync(country.Id));
        }

        [Test]
        public async Task DeleteAllTest()
        {
            await LocationDao.DeleteAllAsync();
            await CountryDao.DeleteAllAsync();
            Assert.AreEqual(0, (await CountryDao.FindAllAsync()).Count());
        }
    }
}