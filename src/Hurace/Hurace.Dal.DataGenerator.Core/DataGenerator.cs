﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Dao;
using Hurace.Dal.DataGenerator.Core.JsonEntities;
using Hurace.Dal.Domain;
using Hurace.Dal.Domain.Interfaces;
using Hurace.Dal.Interface;
using Hurace.Dal.Interface.Base;
using Newtonsoft.Json;

namespace Hurace.Dal.DataGenerator.Core
{
    [ExcludeFromCodeCoverage]
    public class DataGenerator
    {
        private IEnumerable<Country> _countries;
        private IEnumerable<Location> _locations;
        private IEnumerable<Discipline> _disciplines;
        private Season _season;
        private readonly Random _random = new Random();
        private readonly HashSet<DateTime> _usedRaceDates = new HashSet<DateTime>();
        private readonly DateTime _raceBaseDate = new DateTime(2019, 10, 28, 12, 0, 0);
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

        public DataGenerator(string providerName, string connectionString)
        {
            var connectionFactory =
                new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providerName), connectionString);
            var statementFactory = new StatementFactory("hurace");
            _countryDao = new CountryDao(connectionFactory, statementFactory);
            _locationDao = new LocationDao(connectionFactory, statementFactory);
            _skierDao = new SkierDao(connectionFactory, statementFactory);
            _disciplineDao = new DisciplineDao(connectionFactory, statementFactory);
            _raceDao = new RaceDao(connectionFactory, statementFactory);
            _startListDao = new StartListDao(connectionFactory, statementFactory);
            _raceDataDao = new RaceDataDao(connectionFactory, statementFactory);
            _sensorDao = new SensorDao(connectionFactory, statementFactory);
            _timeDataDao = new TimeDataDao(connectionFactory, statementFactory);
            _raceDataDao = new RaceDataDao(connectionFactory, statementFactory);
            _raceEventDao = new RaceEventDao(connectionFactory, statementFactory);
            _skierEventDao = new SkierEventDao(connectionFactory, statementFactory);
            _seasonDao = new SeasonDao(connectionFactory, statementFactory);
        }

        private DateTime GetRandomBirthDate() => _skierBaseDate.AddDays(_random.Next(-300, 300));

        private int GetCountryId(string country) =>
            _countries.FirstOrDefault(c => c.CountryCode.Equals(country))?.Id ?? -1;
        
        private IList<Skier> GenerateSkiers() =>
            LoadJson<SkierJson, Skier>("Data/Skiers.json", skierJson => new Skier
            {
                CountryId = GetCountryId(skierJson.CountryCode),
                GenderId = skierJson.GenderId,
                FirstName = skierJson.FirstName,
                LastName = skierJson.LastName,
                DateOfBirth = GetRandomBirthDate(),
                ImageUrl = skierJson.ImageUrl,
                Retired = false
            });

        private static IList<TResult> LoadJson<T, TResult>(string path, Func<T, TResult> transform)
        {
            using var streamReader = new StreamReader(path);
            var data = JsonConvert.DeserializeObject<List<T>>(streamReader.ReadToEnd());
            return data.Select(transform).ToList();
        }

        private static IEnumerable<Country> GenerateCountries() =>
            LoadJson<CountryJson, Country>("./Data/Countries.json", countryJson => new Country
            {
                CountryCode = countryJson.Code,
                CountryName = countryJson.Name
            });

        private IEnumerable<Location> GenerateLocations() =>
            LoadJson<LocationJson, Location>("./Data/Locations.json", locationJson => new Location
            {
                CountryId = GetCountryId(locationJson.Country),
                LocationName = locationJson.Name
            });

        private IEnumerable<Discipline> GenerateDisciplines() =>
            LoadJson<DisciplineJson, Discipline>("./Data/Disciplines.json", disciplineJson => new Discipline
            {
                DisciplineName = disciplineJson.Name
            });

        private DateTime GetRandomRaceDate()
        {
            var newDate = _raceBaseDate.AddDays(_random.Next(141));
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
                                      GenderId = (int) Domain.Enums.Gender.Male,
                                      DisciplineId = discipline.Id,
                                      SeasonId = _season.Id,
                                      RaceStateId = (int) Domain.Enums.RaceState.Finished,
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
                for (var i = 0; i < 5; i++)
                    yield return new Sensor
                    {
                        RaceId = race.Id,
                        SensorNumber = i
                    };
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
                    EventTypeId = (int) Domain.Enums.RaceDataEvent.RaceStarted,
                    EventDateTime = race.RaceDate
                });

                await _raceEventDao.InsertAsync(new RaceEvent
                {
                    RaceDataId = raceEventId.Value
                });
                
                foreach (var startListSkier in startList.Where(sl => sl.RaceId == race.Id).OrderBy(s => s.StartNumber))
                {
                    var eventId = await _raceDataDao.InsertGetIdAsync(new RaceData
                    {
                        RaceId = race.Id, EventDateTime = raceTime, EventTypeId = (int) Domain.Enums.RaceDataEvent.SkierStarted
                    });

                    await _skierEventDao.InsertAsync(new SkierEvent
                    {
                        RaceId = startListSkier.RaceId,
                        SkierId = startListSkier.SkierId,
                        RaceDataId = eventId.Value
                    });

                    var splitTime = 0;
                    foreach (var sensor in sensors.Where(s => s.RaceId == race.Id))
                    {
                        var raceDataId = await _raceDataDao.InsertGetIdAsync(new RaceData
                        {
                            RaceId = race.Id,
                            EventDateTime = raceTime,
                            EventTypeId = (int) Domain.Enums.RaceDataEvent.SkierSplitTime
                        });

                        var skierEventId = await _skierEventDao.InsertGetIdAsync(new SkierEvent
                        {
                            RaceId = startListSkier.RaceId,
                            SkierId = startListSkier.SkierId,
                            RaceDataId = raceDataId.Value
                        });

                        await _timeDataDao.InsertAsync(new TimeData
                        {
                            Time = splitTime,
                            RaceId = race.Id,
                            SensorId = sensor.Id,
                            SkierId = startListSkier.SkierId,
                            SkierEventId = skierEventId.Value
                        });

                        var splitMillis = GetRandomSplitTime();
                        splitTime += splitMillis;
                        raceTime = raceTime.AddMilliseconds(splitMillis);
                    }

                    eventId = await _raceDataDao.InsertGetIdAsync(new RaceData
                    {
                        RaceId = race.Id,
                        EventTypeId = (int) Domain.Enums.RaceDataEvent.SkierFinished,
                        EventDateTime = raceTime
                    });

                    await _skierEventDao.InsertAsync(new SkierEvent
                    {
                        RaceId = startListSkier.RaceId,
                        SkierId = startListSkier.SkierId,
                        RaceDataId = eventId.Value
                    });
                }

                raceEventId = await _raceDataDao.InsertGetIdAsync(new RaceData
                {
                    RaceId = race.Id,
                    EventTypeId = (int) Domain.Enums.RaceDataEvent.RaceFinished,
                    EventDateTime = raceTime
                });
                await _raceEventDao.InsertAsync(new RaceEvent
                {
                    RaceDataId = raceEventId.Value
                });
            }
        }

        private static async Task PersistEntity<T>(IEnumerable<T> entities, IDefaultCrudDao<T> dao)
            where T : class, ISinglePkEntity, new()
        {
            foreach (var dto in entities) dto.Id = (await dao.InsertGetIdAsync(dto)).Value;
        }

        private static async Task PersistEntity<T>(IEnumerable<T> entities, ICrudDao<T> dao) where T : class, new()
        {
            foreach (var dto in entities) await dao.InsertAsync(dto);
        }

        public async Task Cleanup()
        {
            await _timeDataDao.DeleteAllAsync();
            await _skierEventDao.DeleteAllAsync();
            await _raceEventDao.DeleteAllAsync();
            await _raceDataDao.DeleteAllAsync();
            await _sensorDao.DeleteAllAsync();
            await _startListDao.DeleteAllAsync();
            await _raceDao.DeleteAllAsync();
            await _skierDao.DeleteAllAsync();
            await _locationDao.DeleteAllAsync();
            await _disciplineDao.DeleteAllAsync();
            await _countryDao.DeleteAllAsync();
            await _seasonDao.DeleteAllAsync();
        }
        
        private async Task InsertPossibleDisciplineForSkier(IEnumerable<Skier> skiers)
        {
            foreach (var skier in skiers)
            foreach (var discipline in _disciplines)
                await _skierDao.InsertPossibleDisciplineForSkier(skier.Id, discipline.Id);
        }

        private async Task InsertPossibleDisciplineForLocation()
        {
            foreach (var location in _locations)
            foreach (var discipline in _disciplines)
                await _locationDao.InsertPossibleDisciplineForLocation(location.Id, discipline.Id);
        }
        
        public async Task FillDatabase(int? locationAmount = null,int? raceAmount = null, int? skierAmount = null, int? sensorAmount = null)
        {
            _countries = GenerateCountries();
            await PersistEntity(_countries, _countryDao);
            
            _disciplines = GenerateDisciplines();
            await PersistEntity(_disciplines, _disciplineDao);
            
            _locations = GenerateLocations();
            if (locationAmount != null) _locations = _locations.Take(locationAmount.Value);
            await PersistEntity(_locations, _locationDao);
            await InsertPossibleDisciplineForLocation();
            
            _season = new Season
            {
                StartDate = new DateTime(2019, 10, 28),
                EndDate = new DateTime(2020, 3, 17)
            };
            _season.Id = (await _seasonDao.InsertGetIdAsync(_season)).Value;
            
            var skiers = GenerateSkiers();
            if (skierAmount != null) skiers = skiers.Take(skierAmount.Value).ToList();
            await PersistEntity(skiers, _skierDao);
            await InsertPossibleDisciplineForSkier(skiers);
            
            var races = GenerateRaces().ToList();
            if (raceAmount != null) races = races.Take(raceAmount.Value).ToList();
            await PersistEntity(races, _raceDao);
            
            var sensors = GenerateSensors(races).ToList();
            if (sensorAmount != null) sensors = sensors.Take(sensorAmount.Value).ToList();
            await PersistEntity(sensors, _sensorDao);
            
            var startList = races.SelectMany(r => GenerateStartList(r, skiers)).ToList();
            await PersistEntity(startList, _startListDao);
            
            await GenerateRaceData(races, startList, sensors);
        }
    }
}