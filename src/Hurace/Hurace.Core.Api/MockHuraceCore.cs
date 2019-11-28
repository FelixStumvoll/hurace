using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Core.Api
{
    public class MockHuraceCore : IHuraceCore
    {
        public async Task<IEnumerable<Gender>> GetGenders()
        {
            return new List<Gender> {new Gender {Id = 2, GenderDescription = "Female"},new Gender {Id = 1, GenderDescription = "Male"}};
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

        public async Task<IEnumerable<Race>> GetAllRaces()
        {
            return new List<Race>
            {
                new Race
                {
                    Id = 1,
                    RaceDescription = "Yeet",
                    Discipline = new Discipline {DisciplineName = "Abfahrt"},
                    Gender = new Gender {GenderDescription = "Herren"},
                    Location = new Location {LocationName = "Kitz", Country = new Country()},
                    Season = new Season(),
                    RaceDate = DateTime.Now,
                    RaceState = new RaceState {RaceStateDescription = "Lul"}
                },
                new Race
                {
                    Id = 2,
                    RaceDescription = "Yeet",
                    Discipline = new Discipline {DisciplineName = "Abfahrt"},
                    Gender = new Gender {GenderDescription = "Herren"},
                    Location = new Location {LocationName = "Kitz", Country = new Country()},
                    Season = new Season(),
                    RaceDate = DateTime.Now,
                    RaceState = new RaceState {RaceStateDescription = "Lul"}
                }
            };
        }

        public async Task<IEnumerable<Race>> GetActiveRaces()
        {
            return new List<Race>
            {
                new Race
                {
                    Id = 3,
                    RaceDescription = "LMAO",
                    Discipline = new Discipline {DisciplineName = "Abfahrt"},
                    Gender = new Gender {GenderDescription = "Herren"},
                    Location = new Location {LocationName = "Kitz", Country = new Country()},
                    Season = new Season(),
                    RaceDate = DateTime.Now,
                    RaceState = new RaceState {RaceStateDescription = "Lul"}
                },
                new Race
                {
                    Id = 4,
                    RaceDescription = "Roflkek",
                    Discipline = new Discipline {DisciplineName = "Abfahrt"},
                    Gender = new Gender {GenderDescription = "Herren"},
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