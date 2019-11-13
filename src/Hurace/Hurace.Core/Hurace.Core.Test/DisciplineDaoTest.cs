﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class DisciplineDaoTest : TestBase
    {
        [SetUp]
        public Task BeforeEach() => SetupDiscipline();

        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await DisciplineDao.FindAllAsync()).Count());

        [Test]
        public async Task FindById()
        {
            var discipline = (await DisciplineDao.FindAllAsync()).First();
            var disciplineById = await DisciplineDao.FindByIdAsync(discipline.Id);
            Assert.AreEqual(discipline.DisciplineName, disciplineById?.DisciplineName);
        }
        
        [Test]
        public async Task UpdateTest()
        {
            var discipline = (await DisciplineDao.FindAllAsync()).First();
            discipline.DisciplineName = "ABC";
            await DisciplineDao.UpdateAsync(discipline);
            var disciplineById = await DisciplineDao.FindByIdAsync(discipline.Id);
            Assert.AreEqual("ABC", disciplineById?.DisciplineName);
        }

        [Test]
        public async Task DeleteTest()
        {
            var discipline = (await DisciplineDao.FindAllAsync()).First();
            await DisciplineDao.DeleteAsync(discipline.Id);
            Assert.IsNull(await DisciplineDao.FindByIdAsync(discipline.Id));
        }
        
        [Test]
        public async Task DeleteAllTest()
        {
            await DisciplineDao.DeleteAllAsync();
            Assert.IsEmpty((await DisciplineDao.FindAllAsync()));
        }
    }
}