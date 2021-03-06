﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class SkierDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(5, (await SkierDao.FindAllAsync()).Count());
        
        [Test]
        public async Task FindByIdTest()
        {
            var id = (await SkierDao.FindAllAsync()).First().Id;
            var skier = await SkierDao.FindByIdAsync(id);
            Assert.NotNull(skier);
            Assert.NotNull(skier?.Country);
            Assert.NotNull(skier?.Gender);
        }
        
        [Test]
        public async Task UpdateTest()
        {
            var skier = (await SkierDao.FindAllAsync()).First();
            skier.FirstName = "Testname";
            await SkierDao.UpdateAsync(skier);
            Assert.AreEqual(skier.FirstName, (await SkierDao.FindByIdAsync(skier.Id))?.FirstName);
        }
        
        [Test]
        public async Task InsertTest()
        {
            var countryId = (await CountryDao.FindAllAsync()).First().Id;
            var skierId = await SkierDao.InsertGetIdAsync(new Skier
            {
                CountryId = countryId,
                FirstName = "Test",
                LastName = "pacito",
                GenderId = (int) Domain.Enums.Gender.Male,
                DateOfBirth = new DateTime(1969,4,20)
            });

            var skier = await SkierDao.FindByIdAsync(skierId.Value);
            Assert.AreEqual(countryId, skier.CountryId);
            Assert.AreEqual("Test", skier.FirstName);
            Assert.AreEqual("pacito", skier.LastName);
            Assert.AreEqual((int) Domain.Enums.Gender.Male, skier.GenderId);
            Assert.AreEqual(new DateTime(1969,4,20), skier.DateOfBirth);
            Assert.NotNull(skier.Country);
            Assert.NotNull(skier.Gender);
        }
        
        // [Test]
        // public async Task DeleteTest()
        // {
        //     var id = (await SkierDao.FindAllAsync()).First().Id;
        //     await SkierDao.DeleteAsync(id);
        //     Assert.IsNull(await SkierDao.FindByIdAsync(id));
        // }
        
        // [Test]
        // public async Task DeleteAllTest()
        // {
        //     await SkierDao.DeleteAllAsync();
        //     Assert.AreEqual(0, (await SkierDao.FindAllAsync()).Count());
        // }
        
        [Test]
        public async Task GetPossibleDisciplinesTest()
        {
            var id = (await SkierDao.FindAllAsync()).First().Id;
            Assert.AreEqual(2, (await SkierDao.GetPossibleDisciplinesForSkier(id)).Count());
        }
        
        [Test]
        public async Task AddPossibleDisciplineTest()
        {
            var disciplineId = await DisciplineDao.InsertGetIdAsync(new Discipline
            {
                DisciplineName = "TestDiscipline"
            });
            var skierId = (await SkierDao.FindAllAsync()).First().Id;
            await SkierDao.InsertPossibleDisciplineForSkier(skierId, disciplineId.Value);
            Assert.AreEqual(3, (await SkierDao.GetPossibleDisciplinesForSkier(skierId)).Count());
        }
        
        [Test]
        public async Task DeletePossibleDisciplineTest()
        {
            var skierId = (await SkierDao.FindAllAsync()).First().Id;
            var disciplineId = (await SkierDao.GetPossibleDisciplinesForSkier(skierId)).First().Id;
            await SkierDao.DeletePossibleDisciplineForSkier(skierId, disciplineId);
            Assert.AreEqual(1, (await SkierDao.GetPossibleDisciplinesForSkier(skierId)).Count());
        }
        
        [Test]
        public async Task DeleteAllPossibleDisciplineTest()
        {
            var skierId = (await SkierDao.FindAllAsync()).First().Id;
            await SkierDao.DeleteAllPossibleDisciplineForSkier(skierId);
            Assert.AreEqual(0, (await SkierDao.GetPossibleDisciplinesForSkier(skierId)).Count());
        }
        
        [Test]
        public async Task FindAvailableSkiersForRaceTest()
        {
            var skier = (await SkierDao.FindAllAsync()).First();
            var race = (await RaceDao.FindAllAsync()).First();
            
            var availableSkiers = await SkierDao.FindAvailableSkiersForRace(race.Id);
            Assert.IsTrue(availableSkiers.All(s => s.Id != skier.Id));


            var newSkierId = await SkierDao.InsertGetIdAsync(new Skier
            {
                Retired = false,
                FirstName = "xy",
                LastName = "z",
                CountryId = (await CountryDao.FindAllAsync()).First().Id,
                GenderId = race.GenderId,
                DateOfBirth = DateTime.Today
            });
            
            availableSkiers = await SkierDao.FindAvailableSkiersForRace(race.Id);
            Assert.IsTrue(availableSkiers.All(s => s.Id != newSkierId.Value));

            var newSkier = await SkierDao.FindByIdAsync(newSkierId.Value);

            newSkier.GenderId = newSkier.GenderId == 1 ? 2 : 1;

            await SkierDao.UpdateAsync(newSkier);
            
            availableSkiers = await SkierDao.FindAvailableSkiersForRace(race.Id);
            Assert.IsFalse(availableSkiers.Any(s => s.Id != newSkierId.Value));
        }
    }
}