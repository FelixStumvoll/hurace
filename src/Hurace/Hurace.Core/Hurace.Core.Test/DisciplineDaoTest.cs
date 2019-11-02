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
        private IDisciplineDao _disciplineDao;

        [OneTimeSetUp]
        public void BeforeAll() => _disciplineDao = new DisciplineDao(ConnectionFactory, StatementFactory);

        [SetUp]
        public async Task BeforeEach() => await _disciplineDao.InsertAsync(new Discipline {DisciplineName = "Super-G"});

        [TearDown]
        public async Task AfterEach() => await _disciplineDao.DeleteAllAsync();

        [Test]
        public async Task FindAllTest() => Assert.AreEqual(1, (await _disciplineDao.FindAllAsync()).Count());
    }
}