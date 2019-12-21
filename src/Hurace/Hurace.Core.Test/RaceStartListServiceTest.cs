using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.RaceStartListService;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Moq;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    public class RaceStartListServiceTest
    {
        [Test]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public async Task UpdateStartListTest(bool deleteResult, bool result)
        {
            var inserted = 0;
            var startListDaoMock = new Mock<IStartListDao>();
            startListDaoMock.Setup(sld => sld.DeleteAllForRace(It.IsAny<int>()))
                            .ReturnsAsync(result);
            startListDaoMock.Setup(sld => sld.InsertAsync(It.IsAny<StartList>())).Callback(() => inserted++);

            var startListService = new RaceStartListService(startListDaoMock.Object, null);
            Assert.AreEqual(result, await startListService.UpdateStartList(1, new List<StartList>
            {
                new StartList(),
                new StartList()
            }));
            
            if (deleteResult)
            {
                Assert.AreEqual(2,inserted);
            }
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(0, false)]
        [TestCase(null, false)]
        public async Task IsStartListDefinedTest(int? res, bool result)
        {
            var startListDaoMock = new Mock<IStartListDao>();
            startListDaoMock.Setup(sld => sld.CountStartListForRace(It.IsAny<int>()))
                            .ReturnsAsync(res);
            var startListService = new RaceStartListService(startListDaoMock.Object, null);
            Assert.AreEqual(result, await startListService.IsStartListDefined(1));
        }
    }
}