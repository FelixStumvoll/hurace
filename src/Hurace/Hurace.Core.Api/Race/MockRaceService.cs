using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.Race
{
    public class MockRaceService : IRaceService
    {
        public async Task<IEnumerable<Gender>> GetGenders()
        {
            return new List<Gender>
                {new Gender {Id = 2, GenderDescription = "Female"}, new Gender {Id = 1, GenderDescription = "Male"}};
        }

        public async Task<IEnumerable<Location>> GetLocations()
        {
            return new List<Location>
            {
                new Location {Id = 1, LocationName = "Linz", Country = new Country {CountryName = "Österreich"}}
            };
        }

        public async Task<IEnumerable<Discipline>> GetDisciplines()
        {
            return new List<Discipline>
            {
                new Discipline {Id = 1, DisciplineName = "Downhill"},
                new Discipline {Id = 2, DisciplineName = "Super-G"}
            };
        }

        public async Task<IEnumerable<Dal.Domain.Race>> GetAllRaces()
        {
            return new List<Dal.Domain.Race>
            {
                new Dal.Domain.Race
                {
                    Id = 1,
                    RaceDescription = "Yeet",
                    DisciplineId = 1,
                    Discipline = new Discipline {Id = 1},
                    Gender = new Gender {Id = 1},
                    GenderId = 1,
                    Location = new Location {LocationName = "Kitz", Country = new Country()},
                    Season = new Season(),
                    RaceDate = DateTime.Now,
                    RaceState = new RaceState {RaceStateDescription = "Lul"}
                },
                new Dal.Domain.Race
                {
                    Id = 2,
                    RaceDescription = "Yeet",
                    Discipline = new Discipline {Id = 1},
                    Gender = new Gender {Id = 1},
                    DisciplineId = 1,
                    GenderId = 1,
                    Location = new Location {LocationName = "Kitz", Country = new Country()},
                    Season = new Season(),
                    RaceDate = DateTime.Now,
                    RaceState = new RaceState {RaceStateDescription = "Lul"}
                }
            };
        }

        public async Task<IEnumerable<Dal.Domain.Race>> GetActiveRaces()
        {
            return new List<Dal.Domain.Race>
            {
                new Dal.Domain.Race
                {
                    Id = 3,
                    RaceDescription = "LMAO",
                    Discipline = new Discipline {Id = 1},
                    Gender = new Gender {Id = 1},
                    DisciplineId = 1,
                    GenderId = 1,
                    Location = new Location {LocationName = "Kitz", Country = new Country()},
                    Season = new Season(),
                    RaceDate = DateTime.Now,
                    RaceState = new RaceState {RaceStateDescription = "Lul"}
                },
                new Dal.Domain.Race
                {
                    Id = 4,
                    RaceDescription = "Roflkek",
                    Discipline = new Discipline {Id = 1},
                    Gender = new Gender {Id = 1},
                    DisciplineId = 1,
                    GenderId = 1,
                    Location = new Location {LocationName = "Kitz", Country = new Country()},
                    Season = new Season(),
                    RaceDate = DateTime.Now,
                    RaceState = new RaceState {RaceStateDescription = "Lul"}
                }
            };
        }
        
        public async Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId)
        {
            return Enumerable.Empty<Skier>();
        }

        public async Task<IEnumerable<StartList>> GetStartListForRace(int raceId)
        {
            return Enumerable.Empty<StartList>();
        }
    }
}