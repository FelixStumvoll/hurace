using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class DisciplineDaoTest : TestBase
    {
        [SetUp]
        public async Task BeforeEach() => await SetupDiscipline();

        [TearDown]
        public async Task AfterEach() => await Teardown();

        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await DisciplineDao.FindAllAsync()).Count());

        [Test]
        public async Task FindById()
        {
            var discipline = (await DisciplineDao.FindAllAsync()).First();
            Assert.AreEqual(discipline.DisciplineName,
                            (await DisciplineDao.FindByIdAsync(discipline.Id)).DisciplineName);
        }
    }
}