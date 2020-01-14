using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Service;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Moq;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class ActiveRaceResolverTest
    {
        private static object[] _racesTestSource =
        {
            new object[]
            {
                new List<Race> {new Race(), new Race(),},
            }
        };

        [Test]
        [TestCaseSource(nameof(_racesTestSource))]
        public async Task InitializeActiveRaceResolverTest(List<Race> races)
        {
            var count = 0;
            var mockRaceDao = new Mock<IRaceDao>();

            mockRaceDao.Setup(rd => rd.GetActiveRaces()).ReturnsAsync(races);
            Func<int, IActiveRaceControlService> mockFactory = (int id) =>
            {
                var mockRaceControlService = new Mock<IActiveRaceControlService>();
                mockRaceControlService.Setup(rcs => rcs.InitializeAsync()).Callback(() => count++);
                return mockRaceControlService.Object;
            };

            await new ActiveRaceResolver(mockFactory, mockRaceDao.Object, null, null).InitializeActiveRaceResolver();

            Assert.AreEqual(races.Count, count);
        }
        
        [Test]
        [TestCaseSource(nameof(_racesTestSource))]
        public async Task IndexerTest(List<Race> races)
        {
            var count = 0;
            var mockRaceDao = new Mock<IRaceDao>();

            mockRaceDao.Setup(rd => rd.GetActiveRaces()).ReturnsAsync(races);
            Func<int, IActiveRaceControlService> mockFactory = (int id) =>
            {
                var mockRaceControlService = new Mock<IActiveRaceControlService>();
                mockRaceControlService.Setup(rcs => rcs.InitializeAsync()).Callback(() => count++);
                return mockRaceControlService.Object;
            };

            var service = new ActiveRaceResolver(mockFactory, mockRaceDao.Object, null, null);
            
            await service.InitializeActiveRaceResolver();
            

            Assert.AreEqual(races.Count, count);
        }
    }
}