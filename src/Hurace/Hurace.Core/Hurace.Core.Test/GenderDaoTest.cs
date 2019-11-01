using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dal.Dao;
using Hurace.Dal.Interface;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    public class GenderDaoTest : TestBase
    {
        private IGenderDao _genderDao;

        [OneTimeSetUp]
        public void BeforeAll() => _genderDao = new GenderDao(StatementFactory, ConnectionFactory);

        [Test]
        public async Task FindAllTest() => Assert.AreEqual(2, (await _genderDao.FindAllAsync()).Count());
    }
}