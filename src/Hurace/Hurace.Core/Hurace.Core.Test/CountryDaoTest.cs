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
        private ICountryDao _countryDao;

        [OneTimeSetUp]
        public void BeforeAll()
        {
            _countryDao = new CountryDao(ConnectionFactory, StatementFactory);
        }
        
        [SetUp]
        public async Task BeforeEach()
        {
            await _countryDao.InsertAsync(new Country{CountryCode = "AT", CountryName = "Austria"});
            await _countryDao.InsertAsync(new Country{CountryCode = "DE", CountryName = "Germany"});
            await _countryDao.InsertAsync(new Country{CountryCode = "FR", CountryName = "France"});
        }

        [TearDown]
        public async Task AfterEach() => await _countryDao.DeleteAllAsync();

        [Test]
        public async Task FindAllTest()
        {
            var countries = await _countryDao.FindAllAsync();
            Assert.AreEqual(3, countries.Count());
        }

        [Test]
        public async Task InsertTest()
        {
            await _countryDao.InsertAsync(new Country
            {
                CountryCode = "XY",
                CountryName = "TestCountry"
            });

            var countries = await _countryDao.FindAllAsync();
            Assert.AreEqual(1, countries.Count(c => c.CountryCode.Equals("XY")));
        }

        [Test]
        public async Task InsertGetIdTest()
        {
            var id = await _countryDao.InsertGetIdAsync(new Country
            {
                Id=999,
                CountryCode = "XY",
                CountryName = "TestCountry"
            });
            
            Assert.NotNull(await _countryDao.FindByIdAsync(id));
        }

        [Test]
        public async Task UpdateTest()
        {
            var country = (await _countryDao.FindAllAsync()).First();
            await _countryDao.UpdateAsync(new Country
            {
                Id = country.Id,
                CountryCode = "XX",
                CountryName = "Test"
            });
            var updatedCountry = (await _countryDao.FindByIdAsync(country.Id));
            Assert.AreNotEqual(country.CountryName, updatedCountry.CountryName);
        }

        [Test]
        public async Task FindByIdTest()
        {
            var countries = (await _countryDao.FindAllAsync()).ToList();
            var country = await _countryDao.FindByIdAsync(countries.First().Id);
            Assert.AreEqual(countries.First().Id, country.Id);
        }

        [Test]
        public async Task DeleteTest()
        {
            var country = (await _countryDao.FindAllAsync()).First();
            await _countryDao.DeleteAsync(country.Id);
            Assert.IsNull(await _countryDao.FindByIdAsync(country.Id));
        }

        [Test]
        public async Task DeleteAllTest()
        {
            await _countryDao.DeleteAllAsync();
            Assert.AreEqual(0, (await _countryDao.FindAllAsync()).Count());
        }
    }
}