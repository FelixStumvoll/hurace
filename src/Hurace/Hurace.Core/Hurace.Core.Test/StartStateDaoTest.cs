using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    public class StartStateDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(6, (await StartStateDao.FindAllAsync()).Count());

        [Test]
        public async Task FindById() =>
            Assert.AreEqual("Draw Ready", (await StartStateDao.FindByIdAsync(6)).StartStateDescription);
    }
}