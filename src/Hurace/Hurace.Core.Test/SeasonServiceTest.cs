using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Core.Logic.Services.SeasonService;
using Hurace.Dal.Interface;
using Moq;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class SeasonServiceTest
    {
        private static object[] _deleteSeasonTestSource =
        {
            new object[]
            {
                0, false, false
            },
            new object[]
            {
                5, false, false
            },
            new object[]
            {
                0, true, true
            },
        };
        
        [Test]
        [TestCaseSource(nameof(_deleteSeasonTestSource))]
        public async Task DeleteSeasonTest(int raceCount, bool deleteResult, bool result)
        {
            var mockSeasonDao = new Mock<ISeasonDao>();
            mockSeasonDao.Setup(sd => sd.CountRacesForSeason(It.IsAny<int>())).ReturnsAsync(raceCount);
            mockSeasonDao.Setup(sd => sd.DeleteAsync(It.IsAny<int>())).ReturnsAsync(deleteResult);
            
            var methodResult = await  new SeasonService(mockSeasonDao.Object, null).DeleteSeason(1);
            
            Assert.AreEqual(result, methodResult);
        }
    }
}