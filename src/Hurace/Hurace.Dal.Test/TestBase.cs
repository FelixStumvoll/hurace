using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Dao;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class TestBase
    {
        private StatementFactory StatementFactory { get; } = new StatementFactory("hurace");
        private ConcreteConnectionFactory ConnectionFactory { get; }
        protected IRaceDataDao RaceDataDao { get; set; }
        protected IRaceDao RaceDao { get; set; }
        protected ILocationDao LocationDao { get; set; }
        protected IDisciplineDao DisciplineDao { get; set; }
        protected ISeasonDao SeasonDao { get; set; }
        protected ICountryDao CountryDao { get; set; }
        protected ISkierDao SkierDao { get; set; }
        protected IStartListDao StartListDao { get; set; }
        protected IRaceEventDao RaceEventDao { get; set; }
        protected ISkierEventDao SkierEventDao { get; set; }
        protected ITimeDataDao TimeDataDao { get; set; }
        protected IGenderDao GenderDao { get; set; }
        protected ISensorDao SensorDao { get; set; }
        protected IRaceStateDao RaceStateDao { get; set; }
        protected IStartStateDao StartStateDao { get; set; }
        protected IEventTypeDao EventTypeDao { get; set; }
        
        protected TestBase()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var section = config.GetSection("ConnectionStrings").GetSection("huraceTest");
            
            ConnectionFactory =
                new ConcreteConnectionFactory(DbUtil.GetProviderFactory(section["ProviderName"]), section["ConnectionString"]);

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
            RaceDataDao = new RaceDataDao(ConnectionFactory, StatementFactory);
            RaceStateDao = new RaceStateDao(ConnectionFactory, StatementFactory);
            StartStateDao = new StartStateDao(ConnectionFactory, StatementFactory);
            EventTypeDao = new EventTypeDao(ConnectionFactory, StatementFactory);
        }
        
        #region Setup

        protected async Task SetupTimeData()
        {
            var raceId = await SetupRace();
            var rand = new Random();

            for (var i = 0; i < 5; i++)
            {
                var skierId = await SetupSkier();
                await InsertStartList(skierId, raceId.Value);
                var dt = DateTime.Today;
                for (var j = 0; j < 6; j++)
                {
                    dt = dt.AddMilliseconds(rand.Next(2000, 4000));
                    var sensorId = await InsertSensor(raceId.Value);
                    var raceDataId = await InsertRaceData(raceId.Value, (int) Dal.Domain.Enums.RaceDataEvent.SkierSplitTime);
                    var skierEventId = await InsertSkierEvent(raceDataId.Value, skierId, raceId.Value);
                    await InsertTimeData(raceId.Value, skierId, sensorId.Value, skierEventId.Value, dt.Millisecond);
                }
            }
        }

        protected async Task<int?> SetupRaceEvent()
        {
            var raceId = await SetupRace();
            var raceDataId = await InsertRaceData(raceId.Value, (int) Dal.Domain.Enums.RaceDataEvent.RaceStarted);
            return await InsertRaceEvent(raceDataId.Value);
        }

        protected async Task<int?> SetupSkierEvent()
        {
            var raceId = await SetupRace();
            var countryId = await InsertCountry();
            var skierId = await InsertSkier(countryId.Value);
            await InsertStartList(skierId.Value, raceId.Value);
            var raceDataId = await InsertRaceData(raceId.Value, (int) Dal.Domain.Enums.RaceDataEvent.SkierStarted);
            return await InsertSkierEvent(raceDataId.Value, skierId.Value, raceId.Value);
        }

        protected async Task<int> SetupSkier()
        {
            var countryId = await InsertCountry();
            var disciplineId = await InsertDiscipline();
            var skierId = await InsertSkier(countryId.Value);

            await SkierDao.InsertPossibleDisciplineForSkier(skierId.Value, disciplineId.Value);
            return skierId.Value;
        }

        protected async Task<int?> SetupSensor()
        {
            var raceId = await SetupRace();
            return await InsertSensor(raceId.Value);
        }

        protected async Task<int?> SetupSeason()
        {
            return await InsertSeason();
        }

        protected async Task<int?> SetupRaceData()
        {
            var raceId = await SetupRace();
            return await InsertRaceData(raceId.Value, (int) Dal.Domain.Enums.RaceDataEvent.RaceFinished);
        }

        protected async Task SetupLocation()
        {
            var countryId = await InsertCountry();
            var disciplineId = await InsertDiscipline();

            for (var i = 0; i < 3; i++)
            {
                var lid = await InsertLocation(countryId.Value, $"L{i}");
                await LocationDao.InsertPossibleDisciplineForLocation(lid.Value, disciplineId.Value);
            }
        }

        protected async Task SetupDiscipline() =>
            await InsertDiscipline();

        protected async Task SetupCountry()
        {
            await InsertCountry();
            await InsertCountry("DE", "Germany");
            await InsertCountry("FR", "France");
        }

        protected async Task<int?> SetupRace()
        {
            var seasonId = await InsertSeason();
            var countryId = await InsertCountry();
            var locationId = await InsertLocation(countryId.Value);
            var disciplineId = await InsertDiscipline();
            return await InsertRace(disciplineId.Value, locationId.Value, seasonId.Value);
        }

        protected async Task SetupStartList()
        {
            var skierId = await SetupSkier();
            var raceId = await SetupRace();
            await InsertStartList(skierId, raceId.Value);
        }

        #endregion

        #region Insert

        private async Task<int?> InsertRaceData(int raceId, int type) =>
            await RaceDataDao.InsertGetIdAsync(new RaceData
            {
                EventTypeId = type,
                RaceId = raceId,
                EventDateTime = DateTime.Now
            });

        private Task<int?> InsertCountry(string code = "AT", string name = "Austria") =>
            CountryDao.InsertGetIdAsync(new Country {CountryCode = code, CountryName = name});

        private Task<int?> InsertLocation(int countryId, string locationName = "Kitzbühl") =>
            LocationDao.InsertGetIdAsync(new Location
            {
                CountryId = countryId,
                LocationName = locationName
            });

        private Task<int?> InsertDiscipline(string disciplineName = "Super-G") =>
            DisciplineDao.InsertGetIdAsync(new Discipline {DisciplineName = disciplineName});

        private Task<int?> InsertSeason() => SeasonDao.InsertGetIdAsync(new Season
        {
            EndDate = DateTime.Now.AddDays(1),
            StartDate = DateTime.Now
        });

        private Task<int?> InsertSensor(int raceId) => SensorDao.InsertGetIdAsync(new Sensor
        {
            RaceId = raceId,
            SensorDescription = "Description"
        });

        protected Task InsertStartList(int skierId, int raceId) =>
            StartListDao.InsertAsync(new StartList
            {
                RaceId = raceId,
                SkierId = skierId,
                StartNumber = 1,
                StartStateId = (int) Dal.Domain.Enums.StartState.Finished
            });


        private Task<int?> InsertRaceEvent(int raceDataId) =>
            RaceEventDao.InsertGetIdAsync(new RaceEvent
            {
                RaceDataId = raceDataId
            });

        private Task<int?> InsertSkierEvent(int raceDataId, int skierId, int raceId) => SkierEventDao.InsertGetIdAsync(
            new SkierEvent
            {
                RaceId = raceId,
                SkierId = skierId,
                RaceDataId = raceDataId
            });

        protected Task<int?> InsertSkier(int countryId) => SkierDao.InsertGetIdAsync(new Skier
        {
            CountryId = countryId,
            GenderId = (int) Dal.Domain.Enums.Gender.Male,
            FirstName = "Test",
            LastName = "Pacito",
            DateOfBirth = DateTime.Now
        });

        private Task<int?> InsertRace(int disciplineId, int locationId, int seasonId) =>
            RaceDao.InsertGetIdAsync(new Race
            {
                DisciplineId = disciplineId,
                GenderId = (int) Dal.Domain.Enums.Gender.Male,
                LocationId = locationId,
                RaceDate = DateTime.Now,
                RaceDescription = "Description",
                SeasonId = seasonId,
                RaceStateId = (int) Dal.Domain.Enums.RaceDataEvent.RaceFinished
            });

        private Task InsertTimeData(int raceId, int skierId, int sensorId, int skierEventId, int time) =>
            TimeDataDao.InsertAsync(new TimeData
            {
                RaceId = raceId,
                SkierId = skierId,
                SensorId = sensorId,
                SkierEventId = skierEventId,
                Time = time
            });

        #endregion

        [TearDown]
        [OneTimeSetUp]
        protected async Task Teardown()
        {
            await RaceEventDao.DeleteAllAsync();
            await TimeDataDao.DeleteAllAsync();
            await SkierEventDao.DeleteAllAsync();
            await RaceDataDao.DeleteAllAsync();
            await SensorDao.DeleteAllAsync();
            await StartListDao.DeleteAllAsync();
            await RaceDao.DeleteAllAsync();
            await SkierDao.DeleteAllAsync();
            await LocationDao.DeleteAllAsync();
            await DisciplineDao.DeleteAllAsync();
            await CountryDao.DeleteAllAsync();
            await SeasonDao.DeleteAllAsync();
        }
    }
}