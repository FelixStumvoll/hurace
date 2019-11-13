using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class GenderDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(2, (await GenderDao.FindAllAsync()).Count());

        [Test]
        public async Task FindById() => Assert.AreEqual("male", (await GenderDao.FindByIdAsync(1))?.GenderDescription);
    }
}