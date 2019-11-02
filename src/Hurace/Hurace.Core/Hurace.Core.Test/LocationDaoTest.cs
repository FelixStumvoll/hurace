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
    public class LocationDaoTest : TestBase
    {
        private ILocationDao _locationDao;
        private ICountryDao _countryDao;
        private IDisciplineDao _disciplineDao;
        private int _countryId;
        private int _superGId;
        private int _downhillId;
        
        [OneTimeSetUp]
        public async Task BeforeAll()
        {
            _locationDao = new LocationDao(ConnectionFactory, StatementFactory);
            _countryDao = new CountryDao(ConnectionFactory, StatementFactory);
            _countryId = await _countryDao.InsertGetIdAsync(new Country {CountryCode = "XX", CountryName = "Test"});
            _disciplineDao = new DisciplineDao(ConnectionFactory, StatementFactory);

            _superGId = await _disciplineDao.InsertGetIdAsync(new Discipline {DisciplineName = "Super-G"});
            _downhillId = await _disciplineDao.InsertGetIdAsync(new Discipline {DisciplineName = "Downhill"});
        }

        [OneTimeTearDown]
        public async Task AfterAll()
        {
            await _countryDao.DeleteAllAsync();
            await _disciplineDao.DeleteAllAsync();
        }

        [SetUp]
        public async Task BeforeEach()
        {
            for (var i = 0; i < 3; i++)
            {
                var locationId = await _locationDao.InsertGetIdAsync(new Location
                {
                    CountryId = _countryId,
                    LocationName = $"Location {i}"
                });

                await _locationDao.InsertPossibleDisciplineForLocation(locationId, _superGId);
            }
        }

        [TearDown]
        public async Task AfterEach() => await _locationDao.DeleteAllAsync();

        [Test]
        public async Task GetPossibleDisciplinesTest()
        {
            var location = (await _locationDao.FindAllAsync()).First();
            Assert.AreEqual(1, (await _locationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
        }

        [Test]
        public async Task AddPossibleDisciplineTest()
        {
            var location = (await _locationDao.FindAllAsync()).First();
            await _locationDao.InsertPossibleDisciplineForLocation(location.Id, _downhillId);
            Assert.AreEqual(2,(await _locationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
        }
        
        [Test]
        public async Task RemovePossibleDisciplineTest()
        {
            var location = (await _locationDao.FindAllAsync()).First();
            await _locationDao.DeletePossibleDisciplineForLocation(location.Id, _superGId);
            Assert.AreEqual(0,(await _locationDao.GetPossibleDisciplinesForLocation(location.Id)).Count());
        }

        [Test]
        public async Task GetCountryFromLocation()
        {
            var location = (await _locationDao.FindAllAsync()).First();
            Assert.AreEqual("XX", location.Country.CountryCode);
        }

        [Test]
        public async Task DeleteAllTest()
        {
            await _locationDao.DeleteAllAsync();
            Assert.AreEqual(0, (await _locationDao.FindAllAsync()).Count());
        }
        
        [Test]
        public async Task DeleteTest()
        {
            var id = (await _locationDao.FindAllAsync()).First().Id;
            await _locationDao.DeleteAsync(id);
            Assert.AreEqual(2, (await _locationDao.FindAllAsync()).Count());
        }
    }
}