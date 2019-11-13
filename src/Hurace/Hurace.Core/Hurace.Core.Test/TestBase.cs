using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.QueryBuilder;
using Hurace.Core.Dal.Dao;
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
        public IRaceStateDao RaceStateDao { get; set; }
        public IStartStateDao StartStateDao { get; set; }


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
            RaceDataDao = new RaceDataDao(ConnectionFactory, StatementFactory);
            RaceStateDao = new RaceStateDao(ConnectionFactory, StatementFactory);
            StartStateDao = new StartStateDao(ConnectionFactory, StatementFactory);
        }


        #region Setup

        protected async Task SetupTimeData()
        {
            var raceId = await SetupRace();
            var rand = new Random();

            for (var i = 0; i < 5; i++)
            {
                var skierId = await SetupSkier();
                await InsertStartList(skierId, raceId);
                var dt = DateTime.Today;
                for (var j = 0; j < 6; j++)
                {
                    dt = dt.AddMilliseconds(rand.Next(2000, 4000));
                    var sensorId = await InsertSensor(raceId);
                    var raceDataId = await InsertRaceData(raceId, (int) Constants.SkierEvent.SplitTime);
                    var skierEventId = await InsertSkierEvent(raceDataId, skierId, raceId);
                    await InsertTimeData(raceId, skierId, sensorId, skierEventId, dt);
                }
            }
        }

        protected async Task<int> SetupRaceEvent()
        {
            var raceId = await SetupRace();
            var raceDataId = await InsertRaceData(raceId, (int) Constants.RaceEvent.Started);
            return await InsertRaceEvent(raceDataId);
        }

        protected async Task<int> SetupSkierEvent()
        {
            var raceId = await SetupRace();
            var countryId = await InsertCountry();
            var skierId = await InsertSkier(countryId);
            await InsertStartList(skierId, raceId);
            var raceDataId = await InsertRaceData(raceId, (int) Constants.SkierEvent.Started);
            return await InsertSkierEvent(raceDataId, skierId, raceId);
        }

        protected async Task<int> SetupSkier()
        {
            var countryId = await InsertCountry();
            var disciplineId = await InsertDiscipline();
            var skierId = await InsertSkier(countryId);

            await SkierDao.InsertPossibleDisciplineForSkier(skierId, disciplineId);
            return skierId;
        }

        protected async Task<int> SetupSensor()
        {
            var raceId = await SetupRace();
            return await InsertSensor(raceId);
        }

        protected async Task<int> SetupSeason()
        {
            return await InsertSeason();
        }

        protected async Task<int> SetupRaceData()
        {
            var raceId = await SetupRace();
            return await InsertRaceData(raceId, (int) Constants.RaceEvent.Finished);
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

        protected async Task SetupCountry()
        {
            await InsertCountry();
            await InsertCountry("DE", "Germany");
            await InsertCountry("FR", "France");
        }

        protected async Task<int> SetupRace()
        {
            var seasonId = await InsertSeason();
            var countryId = await InsertCountry();
            var locationId = await InsertLocation(countryId);
            var disciplineId = await InsertDiscipline();
            return await InsertRace(disciplineId, locationId, seasonId);
        }

        protected async Task SetupStartList()
        {
            var skierId = await SetupSkier();
            var raceId = await SetupRace();
            await InsertStartList(skierId, raceId);
        }

        #endregion

        #region Insert

        private async Task<int> InsertRaceData(int raceId, int type)
        {
            return await RaceDataDao.InsertGetIdAsync(new RaceData
            {
                EventTypeId = type,
                RaceId = raceId,
                EventDateTime = DateTime.Now
            });
        }

        private Task<int> InsertCountry(string code = "AT", string name = "Austria") =>
            CountryDao.InsertGetIdAsync(new Country {CountryCode = "XX", CountryName = "Test"});

        private Task<int> InsertLocation(int countryId, string locationName = "Kitzbühl") =>
            LocationDao.InsertGetIdAsync(new Location
            {
                CountryId = countryId,
                LocationName = locationName
            });

        private Task<int> InsertDiscipline(string disciplineName = "Super-G") =>
            DisciplineDao.InsertGetIdAsync(new Discipline {DisciplineName = disciplineName});

        private Task<int> InsertSeason() => SeasonDao.InsertGetIdAsync(new Season
        {
            EndDate = DateTime.Now.AddDays(1),
            StartDate = DateTime.Now
        });

        private Task<int> InsertSensor(int raceId) => SensorDao.InsertGetIdAsync(new Sensor
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
                StartStateId = (int) Constants.StartState.Finished
            });


        private Task<int> InsertRaceEvent(int raceDataId) =>
            RaceEventDao.InsertGetIdAsync(new RaceEvent
            {
                RaceDataId = raceDataId
            });

        private Task<int> InsertSkierEvent(int raceDataId, int skierId, int raceId) => SkierEventDao.InsertGetIdAsync(
            new SkierEvent
            {
                RaceId = raceId,
                SkierId = skierId,
                RaceDataId = raceDataId
            });

        protected Task<int> InsertSkier(int countryId) => SkierDao.InsertGetIdAsync(new Skier
        {
            CountryId = countryId,
            GenderId = (int) Constants.Gender.Male,
            FirstName = "Test",
            LastName = "Pacito",
            DateOfBirth = DateTime.Now
        });

        private Task<int> InsertRace(int disciplineId, int locationId, int seasonId) =>
            RaceDao.InsertGetIdAsync(new Race
            {
                DisciplineId = disciplineId,
                GenderId = (int) Constants.Gender.Male,
                LocationId = locationId,
                RaceDate = DateTime.Now,
                RaceDescription = "Description",
                SeasonId = seasonId,
                RaceStateId = (int) Constants.RaceEvent.Finished
            });

        private Task InsertTimeData(int raceId, int skierId, int sensorId, int skierEventId, DateTime time) =>
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