using System;
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
    public class RaceDaoTest : TestBase
    {
        private IRaceDao _raceDao;
        private ISeasonDao _seasonDao;
        private ILocationDao _locationDao;
        private ICountryDao _countryDao;
        private IDisciplineDao _disciplineDao;
        private int _seasonId;
        private int _locationId;
        private int _disciplineId;

        [OneTimeSetUp]
        public async Task BeforeAll()
        {
            _raceDao = new RaceDao(ConnectionFactory, StatementFactory);
            _seasonDao = new SeasonDao(ConnectionFactory, StatementFactory);
            _locationDao = new LocationDao(ConnectionFactory, StatementFactory);
            _countryDao = new CountryDao(ConnectionFactory, StatementFactory);
            _disciplineDao = new DisciplineDao(ConnectionFactory, StatementFactory);

            _seasonId = await _seasonDao.InsertGetIdAsync(new Season
            {
                EndDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now
            });

            var countryId = await _countryDao.InsertGetIdAsync(new Country {CountryCode = "XX", CountryName = "Test"});

            _locationId = await _locationDao.InsertGetIdAsync(new Location
            {
                CountryId = countryId,
                LocationName = "Lname"
            });

            _disciplineId = await _disciplineDao.InsertGetIdAsync(new Discipline {DisciplineName = "Super-G"});
        }

        [OneTimeTearDown]
        public async Task AfterAll()
        {
            await _raceDao.DeleteAllAsync();
            await _locationDao.DeleteAllAsync();
            await _countryDao.DeleteAllAsync();
            await _disciplineDao.DeleteAllAsync();
        }

        [SetUp]
        public async Task BeforeEach()
        {
            await _raceDao.InsertAsync(new Race
            {
                DisciplineId = _disciplineId,
                GenderId = 1,
                LocationId = _locationId,
                RaceDescription = "Description",
                SeasonId = _seasonId,
                RaceStateId = (int) Constants.RaceEvent.Finished,
                RaceDate = DateTime.Now
            });
        }

        [TearDown]
        public async Task AfterEach() => await _raceDao.DeleteAllAsync();

        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await _raceDao.FindAllAsync()).Count());
    }
}