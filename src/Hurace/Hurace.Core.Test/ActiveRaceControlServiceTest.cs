using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Core.Interface.Configs;
using Hurace.Core.Service;
using Hurace.Core.Timer;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Moq;
using NUnit.Framework;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class ActiveRaceControlServiceTest
    {
        [Test]
        public void InitializeAsyncTest()
        {
            var mockClockProvider = new Mock<IRaceClockProvider>();
            var mockSensorDao = new Mock<ISensorDao>();
            var mockClock = new Mock<IRaceClock>();

            mockClockProvider.Setup(cp => cp.GetRaceClock()).ReturnsAsync(mockClock.Object);
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(2);

            var service = new ActiveRaceControlService(1, null, null, null, null, null, null, mockSensorDao.Object,
                                                       mockClockProvider.Object, null, null);

            Assert.DoesNotThrowAsync(async () => await service.InitializeAsync());
        }

        private static object[] _enableRaceForSkierTestSource =
        {
            new object[]
            {
                null, false
            },
            new object[]
            {
                new StartList {StartStateId = (int) StartState.Upcoming, SkierId = 1, RaceId = 1, StartNumber = 1}, true
            }
        };

        [Test]
        [TestCaseSource(nameof(_enableRaceForSkierTestSource))]
        public async Task EnableRaceForSkierTest(StartList? nextSkier, bool result)
        {
            var mockStartListDao = new Mock<IStartListDao>();
            var mockRaceDataDao = new Mock<IRaceDataDao>();
            var mockSkierEventDao = new Mock<ISkierEventDao>();

            mockStartListDao.Setup(sld => sld.GetNextSkierForRace(It.IsAny<int>())).ReturnsAsync(nextSkier);
            mockStartListDao.Setup(sld => sld.UpdateAsync(It.IsAny<StartList>())).ReturnsAsync(true);
            mockRaceDataDao.Setup(rdd => rdd.InsertGetIdAsync(It.IsAny<RaceData>())).ReturnsAsync(1);
            mockSkierEventDao.Setup(skd => skd.InsertGetIdAsync(It.IsAny<SkierEvent>())).ReturnsAsync(1);

            var service = new ActiveRaceControlService(1, null, mockStartListDao.Object, null, mockRaceDataDao.Object,
                                                       mockSkierEventDao.Object, null, null, null, null, null);

            Assert.AreEqual(result, await service.EnableRaceForSkier());
            if (nextSkier != null)
            {
                Assert.AreEqual((int) StartState.Running, nextSkier.StartStateId);
            }
        }

        private static object[] _cancelSkierTestSource =
        {
            new object[]
            {
                null, false
            },
            new object[]
            {
                new StartList {StartStateId = (int) StartState.Finished, SkierId = 1, RaceId = 1, StartNumber = 1}, true
            }
        };

        [Test]
        [TestCaseSource(nameof(_cancelSkierTestSource))]
        public async Task CancelSkierTest(StartList? skier, bool result)
        {
            var mockActiveRaceService = new Mock<IActiveRaceService>();
            var mockStartListDao = new Mock<IStartListDao>();
            var mockRaceDataDao = new Mock<IRaceDataDao>();
            var mockSkierEventDao = new Mock<ISkierEventDao>();

            mockStartListDao.Setup(sld => sld.FindByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(skier);
            mockStartListDao.Setup(sld => sld.UpdateAsync(It.IsAny<StartList>())).ReturnsAsync(true);
            mockRaceDataDao.Setup(rdd => rdd.InsertGetIdAsync(It.IsAny<RaceData>())).ReturnsAsync(1);
            mockSkierEventDao.Setup(skd => skd.InsertGetIdAsync(It.IsAny<SkierEvent>())).ReturnsAsync(1);

            var service = new ActiveRaceControlService(1, null, mockStartListDao.Object, null, mockRaceDataDao.Object,
                                                       mockSkierEventDao.Object, null, null, null, null,
                                                       mockActiveRaceService.Object);

            Assert.AreEqual(result, await service.CancelSkier(1));
            if (skier != null)
            {
                Assert.AreEqual((int) StartState.Canceled, skier.StartStateId);
            }
        }

        private static object[] _disqualifyCurrentSkierTestSource =
        {
            new object[]
            {
                null, false
            },
            new object[]
            {
                new StartList {StartStateId = (int) StartState.Finished, SkierId = 1, RaceId = 1, StartNumber = 1}, true
            }
        };

        [Test]
        [TestCaseSource(nameof(_disqualifyCurrentSkierTestSource))]
        public async Task DisqualifyCurrentSkierTest(StartList? skier, bool result)
        {
            var mockActiveRaceService = new Mock<IActiveRaceService>();
            var mockStartListDao = new Mock<IStartListDao>();
            var mockRaceDataDao = new Mock<IRaceDataDao>();
            var mockSkierEventDao = new Mock<ISkierEventDao>();

            mockStartListDao.Setup(sld => sld.GetCurrentSkierForRace(It.IsAny<int>())).ReturnsAsync(skier);
            mockStartListDao.Setup(sld => sld.UpdateAsync(It.IsAny<StartList>())).ReturnsAsync(true);
            mockRaceDataDao.Setup(rdd => rdd.InsertGetIdAsync(It.IsAny<RaceData>())).ReturnsAsync(1);
            mockSkierEventDao.Setup(skd => skd.InsertGetIdAsync(It.IsAny<SkierEvent>())).ReturnsAsync(1);

            var service = new ActiveRaceControlService(1, null, mockStartListDao.Object, null, mockRaceDataDao.Object,
                                                       mockSkierEventDao.Object, null, null, null, null,
                                                       mockActiveRaceService.Object);

            Assert.AreEqual(result, await service.DisqualifyCurrentSkier());
            if (skier != null)
            {
                Assert.AreEqual((int) StartState.Disqualified, skier.StartStateId);
            }
        }

        private static object[] _disqualifyFinishedSkierTestSource =
        {
            new object[]
            {
                null, false
            },
            new object[]
            {
                new StartList {StartStateId = (int) StartState.Finished, SkierId = 1, RaceId = 1, StartNumber = 1}, true
            },
            new object[]
            {
                new StartList {StartStateId = (int) StartState.Running, SkierId = 1, RaceId = 1, StartNumber = 1}, false
            }
        };

        [Test]
        [TestCaseSource(nameof(_disqualifyFinishedSkierTestSource))]
        public async Task DisqualifyFinishedSkier(StartList? skier, bool result)
        {
            var mockActiveRaceService = new Mock<IActiveRaceService>();
            var mockStartListDao = new Mock<IStartListDao>();
            var mockRaceDataDao = new Mock<IRaceDataDao>();
            var mockSkierEventDao = new Mock<ISkierEventDao>();

            mockStartListDao.Setup(sld => sld.FindByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(skier);
            mockStartListDao.Setup(sld => sld.UpdateAsync(It.IsAny<StartList>())).ReturnsAsync(true);
            mockRaceDataDao.Setup(rdd => rdd.InsertGetIdAsync(It.IsAny<RaceData>())).ReturnsAsync(1);
            mockSkierEventDao.Setup(skd => skd.InsertGetIdAsync(It.IsAny<SkierEvent>())).ReturnsAsync(1);

            var service = new ActiveRaceControlService(1, null, mockStartListDao.Object, null, mockRaceDataDao.Object,
                                                       mockSkierEventDao.Object, null, null, null, null,
                                                       mockActiveRaceService.Object);

            Assert.AreEqual(result, await service.DisqualifyFinishedSkier(1));
            if (skier != null && result)
            {
                Assert.AreEqual((int) StartState.Disqualified, skier.StartStateId);
            }
        }

        private static object[] _modifyRaceTestSource =
        {
            new object[]
            {
                null, false, null, false, false
            },
            new object[]
            {
                new Race(), false, null, false, false
            },
            new object[]
            {
                new Race(), true, null, false, false
            },
            new object[]
            {
                new Race(), true, 1, false, false
            },
            new object[]
            {
                new Race(), true, 1, true, true
            },
        };

        [Test]
        [TestCaseSource(nameof(_modifyRaceTestSource))]
        public async Task CancelRaceTest(Race? race, bool updateResult, int? raceDataId,
            bool raceEventResult, bool result)
        {
            var mockRaceDao = new Mock<IRaceDao>();
            var mockRaceDataDao = new Mock<IRaceDataDao>();
            var mockRaceEventDao = new Mock<IRaceEventDao>();

            mockRaceDao.Setup(rd => rd.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(race);
            mockRaceDao.Setup(rd => rd.UpdateAsync(It.IsAny<Race>())).ReturnsAsync(updateResult);

            mockRaceDataDao.Setup(rdd => rdd.InsertGetIdAsync(It.IsAny<RaceData>())).ReturnsAsync(raceDataId);
            mockRaceEventDao.Setup(red => red.InsertAsync(It.IsAny<RaceEvent>())).ReturnsAsync(raceEventResult);


            var service = new ActiveRaceControlService(1, mockRaceDao.Object, null, mockRaceEventDao.Object,
                                                       mockRaceDataDao.Object,
                                                       null, null, null, null, null,
                                                       null);

            Assert.AreEqual(result, await service.CancelRace());
        }

        [Test]
        [TestCaseSource(nameof(_modifyRaceTestSource))]
        public async Task StartRaceTest(Race? race, bool updateResult, int? raceDataId,
            bool raceEventResult, bool result)
        {
            var mockRaceDao = new Mock<IRaceDao>();
            var mockRaceDataDao = new Mock<IRaceDataDao>();
            var mockRaceEventDao = new Mock<IRaceEventDao>();

            mockRaceDao.Setup(rd => rd.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(race);
            mockRaceDao.Setup(rd => rd.UpdateAsync(It.IsAny<Race>())).ReturnsAsync(updateResult);

            mockRaceDataDao.Setup(rdd => rdd.InsertGetIdAsync(It.IsAny<RaceData>())).ReturnsAsync(raceDataId);
            mockRaceEventDao.Setup(red => red.InsertAsync(It.IsAny<RaceEvent>())).ReturnsAsync(raceEventResult);


            var service = new ActiveRaceControlService(1, mockRaceDao.Object, null, mockRaceEventDao.Object,
                                                       mockRaceDataDao.Object,
                                                       null, null, null, null, null,
                                                       null);

            Assert.AreEqual(result, await service.StartRace());
        }

        [Test]
        [TestCaseSource(nameof(_modifyRaceTestSource))]
        public async Task EndRaceTest(Race? race, bool updateResult, int? raceDataId,
            bool raceEventResult, bool result)
        {
            var mockRaceDao = new Mock<IRaceDao>();
            var mockRaceDataDao = new Mock<IRaceDataDao>();
            var mockRaceEventDao = new Mock<IRaceEventDao>();

            mockRaceDao.Setup(rd => rd.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(race);
            mockRaceDao.Setup(rd => rd.UpdateAsync(It.IsAny<Race>())).ReturnsAsync(updateResult);

            mockRaceDataDao.Setup(rdd => rdd.InsertGetIdAsync(It.IsAny<RaceData>())).ReturnsAsync(raceDataId);
            mockRaceEventDao.Setup(red => red.InsertAsync(It.IsAny<RaceEvent>())).ReturnsAsync(raceEventResult);

            var service = new ActiveRaceControlService(1, mockRaceDao.Object, null, mockRaceEventDao.Object,
                                                       mockRaceDataDao.Object,
                                                       null, null, null, null, null,
                                                       null);

            Assert.AreEqual(result, await service.EndRace());
        }

        private static object[] _sensorValidationTestSource =
        {
            new object[]
            {
                3, new DateTime(), new StartList(), new DateTime(), 0, new List<TimeData>(), 0, new TimeData(),
                new List<int> {0, 0, 0, 0}, 3, new Sensor(), true
            },
            new object[]
            {
                2, new DateTime(), new StartList(), null, 0, new List<TimeData>(), 0, new TimeData(),
                new List<int> {0, 0, 0, 0}, 3, new Sensor(), false
            },
            new object[]
            {
                0, new DateTime(), new StartList(), new DateTime(), 0, new List<TimeData>(), 0, new TimeData(),
                new List<int> {0}, 3, new Sensor(), true
            },

            new object[]
            {
                -1, new DateTime(), new StartList(), new DateTime(), 0, new List<TimeData>(), 0, new TimeData(),
                new List<int> {0}, 3, new Sensor(), false
            },
            new object[]
            {
                4, new DateTime(), new StartList(), new DateTime(), 0, new List<TimeData>(), 0, new TimeData(),
                new List<int> {0}, 3, new Sensor(), false
            },
            new object[]
            {
                0, new DateTime(), null, new DateTime(), 0, new List<TimeData>(), 0, new TimeData(), new List<int> {0},
                3, new Sensor(), false
            },
            new object[]
            {
                1, new DateTime(), new StartList(), null, 0, new List<TimeData>(), 0, new TimeData(),
                new List<int> {0}, 3, new Sensor(), false
            },
            new object[]
            {
                1, new DateTime(), new StartList(), new DateTime(), 0,
                new List<TimeData> {new TimeData {Sensor = new Sensor {SensorNumber = 1}}}, 0, new TimeData(),
                new List<int> {0, 0}, 3, new Sensor(), false
            },
            new object[]
            {
                1, new DateTime(), new StartList(), new DateTime(), 0,
                new List<TimeData> {new TimeData {Sensor = new Sensor {SensorNumber = 2}}}, 0, new TimeData(),
                new List<int> {0, 0, 0}, 3, new Sensor(), false
            },
        };

        public class MockClock : IRaceClock
        {
            public event TimingTriggeredHandler TimingTriggered;

            public void Trigger(int number, DateTime dateTime) => TimingTriggered?.Invoke(number, dateTime);
        }

        [Test]
        [TestCaseSource(nameof(_sensorValidationTestSource))]
        public async Task TestSensorValidation(int sensorNumber, DateTime sensorDateTime, StartList? currentSkier,
            DateTime? startTime, int average, List<TimeData> currentSkierTimes, int diffToAverage, TimeData? ret,
            List<int> averageAssumption, int lastSensor, Sensor sensor, bool result)
        {
            var mockClockProvider = new Mock<IRaceClockProvider>();
            var mockClock = new MockClock();
            var mockActiveRaceService = new Mock<IActiveRaceService>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            var mockSensorConfig = new SensorConfig(diffToAverage, averageAssumption);
            var mockSensorDao = new Mock<ISensorDao>();
            var mockRaceData = new Mock<IRaceDataDao>();
            var mockRaceEventDao = new Mock<IRaceEventDao>();
            var mockSkierEventDao = new Mock<ISkierEventDao>();
            var mockStartListDao = new Mock<IStartListDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(lastSensor);
            mockSensorDao.Setup(sd => sd.GetSensorForSensorNumber(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(sensor);
            mockActiveRaceService.Setup(rc => rc.GetCurrentSkier(It.IsAny<int>())).ReturnsAsync(currentSkier);
            mockRaceData.Setup(rd => rd.InsertGetIdAsync(It.IsAny<RaceData>())).ReturnsAsync(1);
            mockClockProvider.Setup(provider => provider.GetRaceClock()).ReturnsAsync(mockClock);

            mockSkierEventDao.Setup(skd => skd.InsertGetIdAsync(It.IsAny<SkierEvent>())).ReturnsAsync(1);
            mockTimeDataDao.Setup(tdd => tdd.FindByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(ret);
            mockTimeDataDao.Setup(tdd => tdd.GetStartTimeForStartList(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(startTime);
            mockTimeDataDao.Setup(dd => dd.GetTimeDataForStartList(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(currentSkierTimes);
            var service = new ActiveRaceControlService(1, null, mockStartListDao.Object, mockRaceEventDao.Object,
                                                       mockRaceData.Object,
                                                       mockSkierEventDao.Object, mockTimeDataDao.Object,
                                                       mockSensorDao.Object,
                                                       mockClockProvider.Object,
                                                       mockSensorConfig,
                                                       mockActiveRaceService.Object);

            await service.InitializeAsync();

            var res = false;
            service.OnSplitTime += _ => res = true;
            mockClock.Trigger(sensorNumber, sensorDateTime);

            Assert.AreEqual(result, res);
        }
    }
}