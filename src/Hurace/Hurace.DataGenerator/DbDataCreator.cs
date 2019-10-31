using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using Newtonsoft.Json;

namespace Hurace.DataGenerator
{
    public class DbDataCreator
    {
        private IEnumerable<Country> _countries;
        private IEnumerable<Location> _locations;
        private IEnumerable<Discipline> _disciplines;
        private IEnumerable<Race> _races;
        private Season _season;

        private readonly ICountryDao _countryDao;
        private readonly ILocationDao _locationDao;
        private readonly ISkierDao _skierDao;
        private readonly IDisciplineDao _disciplineDao;
        private readonly IRaceDao _raceDao;
        private readonly IStartListDao _startListDao;
        private readonly IRaceDataDao _raceDataDao;

        private readonly ISeasonDao _seasonDao;
        private QueryFactory _queryFactory;

        public DbDataCreator(string providerName, string connectionString)
        {
            var connectionFactory =
                new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providerName), connectionString, providerName);
            _queryFactory = new QueryFactory("hurace");
            _countryDao = new CountryDao(connectionFactory, _queryFactory);
            _locationDao = new LocationDao(connectionFactory, _queryFactory);
            _skierDao = new SkierDao(connectionFactory, _queryFactory);
            _disciplineDao = new DisciplineDao(connectionFactory, _queryFactory);
            _seasonDao = new SeasonDao(connectionFactory, _queryFactory);
            _raceDao = new RaceDao(connectionFactory, _queryFactory);
            _startListDao = new StartListDao(connectionFactory, _queryFactory);
            _raceDataDao = new RaceDataDao(connectionFactory, _queryFactory);
        }

        private async Task LoadFixedData()
        {
            _countries = await _countryDao.FindAllAsync();
            _locations = await _locationDao.FindAllAsync();
            _disciplines = await _disciplineDao.FindAllAsync();
        }

        private static DateTime GetRandomBirthDate()
        {
            var baseDate = new DateTime(1985, 1, 1);
            var rand = new Random();
            return baseDate.AddDays(rand.Next(-300, 300));
        }

        private int GetCountryId(string country) =>
            _countries.FirstOrDefault(c => c.CountryName.Equals(country))?.Id ?? -1;

        private static int GetGenderId(string gender) =>
            gender switch
            {
                "f" => 2,
                "m" => 1,
                _ => -1,
            };

        private static (string firstname, string lastname) GetName(string name)
        {
            var spaceIndex = name.IndexOf(" ", StringComparison.CurrentCulture);
            return (name.Substring(0, spaceIndex), name.Substring(spaceIndex + 1));
        }

        private IEnumerable<Skier> GenerateSkier()
        {
            using var streamReader = new StreamReader("Data.json");
            var json = streamReader.ReadToEnd();
            var skier = JsonConvert.DeserializeObject<List<JsonData>>(json);

            return skier.Select(jdata =>
            {
                var (firstname, lastname) = GetName(jdata.Name);
                return new Skier
                {
                    CountryId = GetCountryId(jdata.Country),
                    GenderId = GetGenderId(jdata.Gender),
                    FirstName = firstname,
                    LastName = lastname,
                    DateOfBirth = GetRandomBirthDate()
                };
            });
        }

        private static DateTime GetRandomRaceDate()
        {
            var baseDate = new DateTime(2018, 10, 28);
            var rand = new Random();
            return baseDate.AddDays(rand.Next(160));
        }

        private IEnumerable<Race> GenerateRaces() =>
            _locations.SelectMany(location => _disciplines,
                                  (location, discipline) => new Race
                                  {
                                      GenderId = 1,
                                      DisciplineId = discipline.Id,
                                      SeasonId = 2,
                                      RaceStateId = 3,
                                      RaceDate = GetRandomRaceDate(),
                                      LocationId = location.Id,
                                      RaceDescription =
                                          $"{location.LocationName} {discipline.DisciplineName}"
                                  });

        private static IEnumerable<StartList> GenerateStartList(Race race, IEnumerable<Skier> skier)
        {
            var startNumber = 1;
            return skier.Where(s => s.GenderId == race.GenderId)
                .Select(s => new StartList
                {
                    RaceId = race.Id,
                    SkierId = s.Id,
                    StartNumber = startNumber++
                });
        }
        
        private async Task GenerateRaceData(IEnumerable<Race> races, IEnumerable<Skier> skier)
        {
            var skierList = skier.ToList();
            foreach (var race in races)
            {
                foreach (var startList in GenerateStartList(race, skierList))
                    await _startListDao.InsertAsync(startList);

                await _raceDataDao.InsertAsync(new RaceData
                {
                    RaceId = race.Id,
                    EventTypeId = 1
                });
            }
        }

        private static async Task PersistEntity<T>(IEnumerable<T> entities, IBaseDao<T> dao) where T : class
        {
            foreach (var dto in entities) await dao.InsertAsync(dto);
        }

        private async Task Cleanup()
        {
            await _skierDao.DeleteAllAsync();
            await _raceDao.DeleteAllAsync();
        }

        public async Task FillDatabase()
        {
//            await LoadFixedData();
//            var skier = GenerateSkier();
//            var races = GenerateRaces();
//            await PersistEntity(skier, _skierDao);
//            await PersistEntity(races, _raceDao);
//            await Cleanup();
        }
    }
}