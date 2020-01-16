using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Core.Service;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Moq;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class ActiveRaceResolverTest
    {
        private static object[] _initializeActiveRaceResolverTestSource =
        {
            new object[]
            {
                new List<Race> {new Race(), new Race(),},
            }
        };

        [Test]
        [TestCaseSource(nameof(_initializeActiveRaceResolverTestSource))]
        public async Task InitializeActiveRaceResolverTest(List<Race> races)
        {
            var count = 0;
            var mockRaceDao = new Mock<IRaceDao>();

            mockRaceDao.Setup(rd => rd.GetActiveRaces()).ReturnsAsync(races);
            Func<int, IActiveRaceControlService> mockFactory = id =>
            {
                var mockRaceControlService = new Mock<IActiveRaceControlService>();
                mockRaceControlService.Setup(rcs => rcs.InitializeAsync()).Callback(() => count++);
                return mockRaceControlService.Object;
            };

            await new ActiveRaceResolver(mockFactory, mockRaceDao.Object).InitializeActiveRaceResolver();

            Assert.AreEqual(races.Count, count);
        }

        [Test]
        [TestCase(true, false, 100, null)]
        [TestCase(false, false, 100, null)]
        [TestCase(false, true, 100, 100)]
        public async Task StartRaceTest(bool serviceNull, bool startResult, int inputId, int? outputId)
        {
            Func<int, IActiveRaceControlService> mockFactory = id =>
            {
                var mockControlService = new Mock<IActiveRaceControlService>();
                mockControlService.Setup(foo => foo.StartRace()).ReturnsAsync(startResult);
                mockControlService.Setup(cs => cs.RaceId).Returns(inputId);
                return serviceNull ? null : mockControlService.Object;
            };

            var service = new ActiveRaceResolver(mockFactory, null);

            var methodResult = await service.StartRace(inputId);

            Assert.AreEqual(outputId, methodResult?.RaceId);
        }

        [Test]
        public async Task StartRaceFinishEventTest()
        {
            static IActiveRaceControlService MockFactory(int id)
            {
                var mockControlService = new Mock<IActiveRaceControlService>();
                mockControlService.Setup(foo => foo.StartRace()).ReturnsAsync(true);
                mockControlService.Setup(cs => cs.RaceId).Returns(1);
                mockControlService.Setup(cs => cs.EndRace())
                                  .Callback(() => mockControlService.Raise(x => x.OnRaceFinished += null, new Race {Id = 1}));
                return mockControlService.Object;
            }

            var service = new ActiveRaceResolver(MockFactory, null);

            var methodResult = await service.StartRace(1);
            Assert.NotNull(service[1]);

            await methodResult.EndRace();

            Assert.IsNull(service[1]);
        }
        
        [Test]
        public async Task StartRaceCanceledEventTest()
        {
            static IActiveRaceControlService MockFactory(int id)
            {
                var mockControlService = new Mock<IActiveRaceControlService>();
                mockControlService.Setup(foo => foo.StartRace()).ReturnsAsync(true);
                mockControlService.Setup(cs => cs.RaceId).Returns(1);
                mockControlService.Setup(cs => cs.CancelRace())
                                  .Callback(() => mockControlService.Raise(x => x.OnRaceCancelled += null, new Race {Id = 1}));
                return mockControlService.Object;
            }

            var service = new ActiveRaceResolver(MockFactory, null);

            var methodResult = await service.StartRace(1);
            Assert.NotNull(service[1]);

            await methodResult.CancelRace();

            Assert.IsNull(service[1]);
        }
    }
}