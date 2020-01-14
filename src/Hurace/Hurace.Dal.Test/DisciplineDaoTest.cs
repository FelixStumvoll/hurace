using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using NUnit.Framework;

namespace Hurace.Dal.Test
{
    [ExcludeFromCodeCoverage]
    public class DisciplineDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(2, (await DisciplineDao.FindAllAsync()).Count());

        [Test]
        public async Task FindById()
        {
            var discipline = (await DisciplineDao.FindAllAsync()).First();
            var disciplineById = await DisciplineDao.FindByIdAsync(discipline.Id);
            Assert.AreEqual(discipline.DisciplineName, disciplineById?.DisciplineName);
        }
        
        [Test]
        public async Task InsertTest()
        {
            var disciplineId = await DisciplineDao.InsertGetIdAsync(new Discipline
            {
                DisciplineName = "XXX"
            });
            var disciplineById = await DisciplineDao.FindByIdAsync(disciplineId.Value);
            Assert.AreEqual("XXX", disciplineById?.DisciplineName);
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
            var disciplineId = await DisciplineDao.InsertGetIdAsync(new Discipline {DisciplineName = "E"});
            await DisciplineDao.DeleteAsync(disciplineId.Value);
            Assert.IsNull(await DisciplineDao.FindByIdAsync(disciplineId.Value));
        }
        
        [Test]
        public async Task DeleteAllTest()
        {
            await DbTeardown();
            await DisciplineDao.InsertAsync(new Discipline {DisciplineName = "E"});
            await DisciplineDao.DeleteAllAsync();
            Assert.IsEmpty(await DisciplineDao.FindAllAsync());
        }
    }
}