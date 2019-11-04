using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using Hurace.Dal.Interface.Util;
using Newtonsoft.Json;

namespace Hurace.DataGenerator
{
    [ExcludeFromCodeCoverage]
    public class DbDataCreator
    {
        private IEnumerable<Country> _countries;
        private IEnumerable<Location> _locations;
        private IEnumerable<Discipline> _disciplines;
        private IEnumerable<Race> _races;
        private Season _season;

        private readonly Random _random = new Random();
        private readonly HashSet<DateTime> _usedRaceDates = new HashSet<DateTime>();
        private readonly DateTime _raceBaseDate = new DateTime(2018, 10, 28, 12, 0, 0);
        private readonly DateTime _skierBaseDate = new DateTime(1985, 1, 1);
        private const int BaseSplitTime = 20000;
        private const int SplitTimeVariation = 1000;

        private readonly ICountryDao _countryDao;
        private readonly ILocationDao _locationDao;
        private readonly ISkierDao _skierDao;
        private readonly IDisciplineDao _disciplineDao;
        private readonly IRaceDao _raceDao;
        private readonly IStartListDao _startListDao;
        private readonly IRaceDataDao _raceDataDao;
        private readonly ISensorDao _sensorDao;
        private readonly ITimeDataDao _timeDataDao;
        private readonly IRaceEventDao _raceEventDao;
        private readonly ISkierEventDao _skierEventDao;

        private readonly ISeasonDao _seasonDao;
        private StatementFactory _statementFactory;

        public DbDataCreator(string providerName, string connectionString)
        {
            var connectionFactory =
                new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providerName), connectionString, providerName);
            _statementFactory = new StatementFactory("hurace");
            _countryDao = new CountryDao(connectionFactory, _statementFactory);
            _locationDao = new LocationDao(connectionFactory, _statementFactory);
            _skierDao = new SkierDao(connectionFactory, _statementFactory);
            _disciplineDao = new DisciplineDao(connectionFactory, _statementFactory);
            _seasonDao = new SeasonDao(connectionFactory, _statementFactory);
            _raceDao = new RaceDao(connectionFactory, _statementFactory);
            _startListDao = new StartListDao(connectionFactory, _statementFactory);
            _raceDataDao = new RaceDataDao(connectionFactory, _statementFactory);
            _sensorDao = new SensorDao(connectionFactory, _statementFactory);
            _timeDataDao = new TimeDataDao(connectionFactory, _statementFactory);
            _raceDataDao = new RaceDataDao(connectionFactory, _statementFactory);
            _raceEventDao = new RaceEventDao(connectionFactory, _statementFactory);
            _skierEventDao = new SkierEventDao(connectionFactory, _statementFactory);
        }

        private async Task LoadFixedData()
        {
            _countries = await _countryDao.FindAllAsync();
            _locations = await _locationDao.FindAllAsync();
            _disciplines = await _disciplineDao.FindAllAsync();
        }

        private DateTime GetRandomBirthDate()
        {
            return _skierBaseDate.AddDays(_random.Next(-300, 300));
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

            return skier.Select(jsonData =>
            {
                var (firstname, lastname) = GetName(jsonData.Name);
                return new Skier
                {
                    CountryId = GetCountryId(jsonData.Country),
                    GenderId = GetGenderId(jsonData.Gender),
                    FirstName = firstname,
                    LastName = lastname,
                    DateOfBirth = GetRandomBirthDate()
                };
            });
        }


        private DateTime GetRandomRaceDate()
        {
            var newDate = _raceBaseDate.AddDays(_random.Next(160));
            if (_usedRaceDates.Contains(newDate)) GetRandomRaceDate();
            _usedRaceDates.Add(newDate);
            return newDate;
        }

        private int GetRandomSplitTime() =>
            _random.Next(-SplitTimeVariation, SplitTimeVariation) + BaseSplitTime;

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
                            StartNumber = startNumber++,
                            StartStateId = 3
                        });
        }

        private static IEnumerable<Sensor> GenerateSensors(IEnumerable<Race> races)
        {
            foreach (var race in races)
            {
                for (var i = 0; i < 5; i++)
                {
                    yield return new Sensor
                    {
                        RaceId = race.Id,
                        SensorDescription = $"{race.RaceDescription} - Sensor:{i}"
                    };
                }
            }
        }

        private async Task GenerateRaceData(IEnumerable<Race> races, IEnumerable<StartList> startList,
            IEnumerable<Sensor> sensors)
        {
            startList = startList.ToList();
            sensors = sensors.ToList();
            foreach (var race in races)
            {
                var raceTime = race.RaceDate;
                var raceEventId = await _raceDataDao.InsertGetIdAsync(new RaceData
                {
                    RaceId = race.Id,
                    EventTypeId = (int) Constants.RaceEvent.Started,
                    EventDateTime = race.RaceDate
                });

                await _raceEventDao.InsertAsync(new RaceEvent
                {
                    RaceDataId = raceEventId
                });
                foreach (var startListSkier in startList.Where(sl => sl.RaceId == race.Id).OrderBy(s => s.StartNumber))
                {
                    var eventId = await _raceDataDao.InsertGetIdAsync(new RaceData
                    {
                        RaceId = race.Id, EventDateTime = raceTime, EventTypeId = (int)Constants.SkierEvent.Started
                    });

                    await _skierEventDao.InsertAsync(new SkierEvent
                    {
                        RaceId = startListSkier.RaceId,
                        SkierId = startListSkier.SkierId,
                        RaceDataId = eventId
                    });

                    var splitTime = race.RaceDate.AddMinutes(-race.RaceDate.Minute).AddHours(-race.RaceDate.Hour);
                    foreach (var sensor in sensors.Where(s => s.RaceId == race.Id))
                    {
                        var raceDataId = await _raceDataDao.InsertGetIdAsync(new RaceData
                        {
                            RaceId = race.Id,
                            EventDateTime = raceTime,
                            EventTypeId = (int) Constants.SkierEvent.SplitTime
                        });
                        
                        await _timeDataDao.InsertAsync(new TimeData
                        {
                            Time = splitTime,
                            RaceId = race.Id,
                            SensorId = sensor.Id,
                            SkierId = startListSkier.SkierId,
                            SkierEventId = raceDataId
                        });

                        var splitSeconds = GetRandomSplitTime();
                        splitTime = splitTime.AddMilliseconds(splitSeconds);
                        raceTime = raceTime.AddMilliseconds(splitSeconds);
                    }

                    eventId = await _raceDataDao.InsertGetIdAsync(new RaceData
                    {
                        RaceId = race.Id, 
                        EventTypeId = (int) Constants.SkierEvent.Finished,
                        EventDateTime = raceTime
                    });

                    await _skierEventDao.InsertAsync(new SkierEvent
                    {
                        RaceId = startListSkier.RaceId,
                        SkierId =  startListSkier.SkierId,
                        RaceDataId = eventId
                    });
                }

                raceEventId = await _raceDataDao.InsertGetIdAsync(new RaceData
                {
                    RaceId = race.Id, 
                    EventTypeId = (int) Constants.RaceEvent.Finished,
                    EventDateTime = raceTime
                });
                await _raceEventDao.InsertAsync(new RaceEvent
                {
                    RaceDataId = raceEventId
                });
            }
        }

        private static async Task PersistEntity<T>(IEnumerable<T> entities, ICrudDao<T> dao) where T : class, new()
        {
            foreach (var dto in entities) await dao.InsertAsync(dto);
        }

        public async Task Cleanup()
        {
            await _timeDataDao.DeleteAllAsync();
            await _raceDataDao.DeleteAllAsync();
            await _sensorDao.DeleteAllAsync();
            await _startListDao.DeleteAllAsync();
            await _raceDao.DeleteAllAsync();
            await _skierDao.DeleteAllAsync();
        }

        public async Task FillDatabase()
        {
            await LoadFixedData();
            var skiers = GenerateSkier();
            await PersistEntity(skiers, _skierDao);
            skiers = await _skierDao.FindAllAsync();

            var races = GenerateRaces();
            await PersistEntity(races, _raceDao);
            races = await _raceDao.FindAllAsync();

            var sensors = GenerateSensors(races);
            await PersistEntity(sensors, _sensorDao);
            sensors = await _sensorDao.FindAllAsync();

            var startList = races.SelectMany(r => GenerateStartList(r, skiers)).ToList();
            await PersistEntity(startList, _startListDao);

            await GenerateRaceData(races, startList, sensors);
        }
    }
}