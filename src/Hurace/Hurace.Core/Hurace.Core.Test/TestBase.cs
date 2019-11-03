using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class TestBase
    {
        private const string ConnectionString =
            "Data Source=localhost;Initial Catalog=huraceDB;Persist Security Info=True;User ID=SA;Password=EHq(iT|$@A4q";

        private const string ProviderName = "Microsoft.Data.SqlClient";
        protected StatementFactory StatementFactory { get; } = new StatementFactory("hurace");
        protected ConcreteConnectionFactory ConnectionFactory { get; }
        protected IRaceDataDao RaceDataDao { get; set; }
        public IRaceDao RaceDao { get; set; }
        public ILocationDao LocationDao { get; set; }
        public IDisciplineDao DisciplineDao { get; set; }
        public ISeasonDao SeasonDao { get; set; }
        public ICountryDao CountryDao { get; set; }
        public ISkierDao SkierDao { get; set; }
        public IStartListDao StartListDao { get; set; }
        public IRaceEventDao RaceEventDao { get; set; }
        public ISkierEventDao SkierEventDao { get; set; }
        public ITimeDataDao TimeDataDao { get; set; }
        public IGenderDao GenderDao { get; set; }
        public ISensorDao SensorDao { get; set; }
        

        protected TestBase()
        {
            ConnectionFactory =
                new ConcreteConnectionFactory(DbUtil.GetProviderFactory(ProviderName), ConnectionString, ProviderName);
            
            RaceDao = new RaceDao(ConnectionFactory, StatementFactory);
            SeasonDao = new SeasonDao(ConnectionFactory, StatementFactory);
            LocationDao = new LocationDao(ConnectionFactory, StatementFactory);
            CountryDao = new CountryDao(ConnectionFactory, StatementFactory);
            DisciplineDao = new DisciplineDao(ConnectionFactory, StatementFactory);
            SkierDao = new SkierDao(ConnectionFactory, StatementFactory);
            StartListDao = new StartListDao(ConnectionFactory, StatementFactory);
            RaceEventDao = new RaceEventDao(ConnectionFactory, StatementFactory);
            SkierEventDao = new SkierEventDao(ConnectionFactory, StatementFactory);
            TimeDataDao = new TimeDataDao(ConnectionFactory, StatementFactory);
            GenderDao = new GenderDao(ConnectionFactory, StatementFactory);
            SensorDao = new SensorDao(ConnectionFactory, StatementFactory);
        }


        protected async Task SetupLocation()
        {
            var countryId = await InsertCountry();
            var disciplineId = await InsertDiscipline();

            for (var i = 0; i < 3; i++)
            {
                var lid = await InsertLocation(countryId, $"L{i}");
                await LocationDao.InsertPossibleDisciplineForLocation(lid, disciplineId);
            }
        }

        protected async Task SetupDiscipline() =>
            await InsertDiscipline();

        private async Task<int> InsertCountry(string code = "AT", string name = "Austria") => 
            await CountryDao.InsertGetIdAsync(new Country {CountryCode = "XX", CountryName = "Test"});

        private async Task<int> InsertLocation(int countryId, string locationName = "Kitzbühl") => 
            await LocationDao.InsertGetIdAsync(new Location
            {
                CountryId = countryId,
                LocationName = locationName
            });

        private async Task<int> InsertDiscipline(string disciplineName = "Super-G") => 
            await DisciplineDao.InsertGetIdAsync(new Discipline {DisciplineName = disciplineName});
        
        protected async Task SetupRace()
        {
            var seasonId = await SeasonDao.InsertGetIdAsync(new Season
            {
                EndDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now
            });

            var countryId = await InsertCountry();

            var locationId = await InsertLocation(countryId);

            var disciplineId = await InsertDiscipline();
            await RaceDao.InsertGetIdAsync(new Race
            {
                DisciplineId = disciplineId,
                GenderId = (int) Constants.Gender.Male,
                LocationId = locationId,
                RaceDate = DateTime.Now,
                RaceDescription = "Description",
                SeasonId = seasonId,
                RaceStateId = (int) Constants.RaceEvent.Finished
            });
        }

        protected async Task SetupCountry()
        {
            await InsertCountry();
            await InsertCountry("DE", "Germany");
            await InsertCountry("FR", "France");
        }

        protected async Task Teardown()
        {
            await RaceEventDao.DeleteAllAsync();
            await TimeDataDao.DeleteAllAsync();
            await SkierEventDao.DeleteAllAsync();
            await RaceEventDao.DeleteAllAsync();
            await SensorDao.DeleteAllAsync();
            await StartListDao.DeleteAllAsync();
            await RaceDao.DeleteAllAsync();
            await LocationDao.DeleteAllAsync();
            await DisciplineDao.DeleteAllAsync();
            await SkierDao.DeleteAllAsync();
            await CountryDao.DeleteAllAsync();
            await SeasonDao.DeleteAllAsync();
        }
//        [SetUp]
//        public async Task BeforeEach()
//        {
//            var countryId = await CountryDao.InsertGetIdAsync(new Country{CountryCode = "AT", CountryName = "Austria"});
//            await CountryDao.InsertAsync(new Country{CountryCode = "DE", CountryName = "Germany"});
//            await CountryDao.InsertAsync(new Country{CountryCode = "FR", CountryName = "France"});
//            
//            var disciplineId = await DisciplineDao.InsertGetIdAsync(new Discipline {DisciplineName = "Super-G"});
//            var locationId = await LocationDao.InsertGetIdAsync(new Location
//            {
//                CountryId = countryId,
//                LocationName = "Location"
//            });
//            var seasonId = await SeasonDao.InsertGetIdAsync(new Season
//            {
//                StartDate = DateTime.Now.AddDays(-1),
//                EndDate = DateTime.Now.AddDays(1)
//                
//            });
//            await LocationDao.InsertPossibleDisciplineForLocation(locationId, disciplineId);
//            await RaceDao.InsertAsync(new Race
//            {
//                DisciplineId = disciplineId,
//                GenderId = (int)Constants.Gender.Male,
//                LocationId = locationId,
//                RaceDescription = "Description",
//                RaceStateId = (int)Constants.RaceState.Finished,
//                SeasonId = seasonId,
//                RaceDate = DateTime.Now
//            });
//        }
//
//        [TearDown]
//        public async Task AfterEach()
//        {
//            await RaceEventDao.DeleteAllAsync();
//            await TimeDataDao.DeleteAllAsync();
//            await SkierEventDao.DeleteAllAsync();
//            await RaceDataDao.DeleteAllAsync();
//            await SensorDao.DeleteAllAsync();
//            await StartListDao.DeleteAllAsync();
//            await RaceDao.DeleteAllAsync();
//            await SeasonDao.DeleteAllAsync();
//            await LocationDao.DeleteAllAsync();
//            await SkierDao.DeleteAllAsync();
//            await CountryDao.DeleteAllAsync();
//        }
    }
}