using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Core.Service;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Moq;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class SkierServiceTest
    {
        private static object[] _updatePossibleDisciplineTestSource =
        {
            new object[]
            {
                new Skier(), new List<int> {1, 2, 3}, 3, true
            },
            new object[]
            {
                null, new List<int> {1, 2, 3}, 0, false
            },
            new object[]
            {
                new Skier(), new List<int> (), 0, true
            },
        };
        
        [Test]
        [TestCaseSource(nameof(_updatePossibleDisciplineTestSource))]
        public async Task UpdatePossibleDiscipleTest(Skier? skier, IEnumerable<int> disciplines, int countResult, bool result)
        {
            var mockSkierDao = new Mock<ISkierDao>();
            var count = 0;
            mockSkierDao.Setup(sd => sd.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(skier);
            mockSkierDao.Setup(sd => sd.DeleteAllPossibleDisciplineForSkier(It.IsAny<int>())).ReturnsAsync(true);
            mockSkierDao.Setup(sd => sd.InsertPossibleDisciplineForSkier(It.IsAny<int>(), It.IsAny<int>()))
                        .Callback(() => count++);
            
            var updateResult = await new SkierService(mockSkierDao.Object, null, null).UpdatePossibleDisciplines(100, disciplines);
            
            Assert.AreEqual(result,updateResult );
            Assert.AreEqual(countResult, count);
        }
    }
}