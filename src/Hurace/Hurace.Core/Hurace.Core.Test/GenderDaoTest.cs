using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Dal.Dao;
using Hurace.Dal.Interface;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class GenderDaoTest : TestBase
    {
        [Test]
        public async Task FindAllTest() => Assert.AreEqual(2, (await GenderDao.FindAllAsync()).Count());
    }
}