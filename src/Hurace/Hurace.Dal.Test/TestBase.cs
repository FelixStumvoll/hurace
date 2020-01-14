using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Dao;
using Hurace.Dal.DataGenerator.Core;
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

        private DataGenerator.Core.DataGenerator _dataGenerator;

        protected TestBase()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var section = config.GetSection("ConnectionStrings").GetSection("huraceTest");

            var provider = section["ProviderName"];
            var connectionString = section["ConnectionString"];
            
            ConnectionFactory =
                new ConcreteConnectionFactory(DbUtil.GetProviderFactory(provider), connectionString);
            
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
            _dataGenerator = new DataGenerator.Core.DataGenerator(provider, connectionString);
        }

        [SetUp]
        protected async Task DbSetup() => 
            await _dataGenerator.FillDatabase(1, 2, 5, 2);

        [TearDown]
        protected async Task DbTeardown() => 
            await _dataGenerator.Cleanup();
    }
}