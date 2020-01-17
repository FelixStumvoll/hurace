using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Service;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Moq;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class RaceStatServiceTest
    {
        [Test]
        public async Task GetFinishedSkierRankingTestNoSensor()
        {
            var mockSensorDao = new Mock<ISensorDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync((int?) null);
            Assert.AreEqual(
                null,
                await new RaceStatService(null, null, mockSensorDao.Object, null)
                    .GetFinishedSkierRanking(1));
        }

        [Test]
        public async Task GetFinishedSkierRankingTest()
        {
            var mockSensorDao = new Mock<ISensorDao>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(3);
            mockTimeDataDao.Setup(sld => sld.GetRankingForSensor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<TimeData>
                           {
                               new TimeData {Time = 0, StartList = new StartList {SkierId = 1}},
                               new TimeData {Time = 3, StartList = new StartList {SkierId = 2}},
                               new TimeData {Time = 3, StartList = new StartList {SkierId = 3}},
                               new TimeData {Time = 4, StartList = new StartList {SkierId = 4}},
                           });

            var ranking = (await new RaceStatService(null, mockTimeDataDao.Object, mockSensorDao.Object, null)
                .GetFinishedSkierRanking(1)).ToList();

            Assert.AreEqual(1, ranking[0].Position);
            Assert.AreEqual(1, ranking[0].StartList.SkierId);

            Assert.AreEqual(2, ranking[1].Position);
            Assert.AreEqual(2, ranking[1].StartList.SkierId);

            Assert.AreEqual(2, ranking[2].Position);
            Assert.AreEqual(3, ranking[2].StartList.SkierId);

            Assert.AreEqual(4, ranking[3].Position);
            Assert.AreEqual(4, ranking[3].StartList.SkierId);
        }

        [Test]
        [TestCase(-5, 5, 10)]
        [TestCase(5, 10, 5)]
        [TestCase(0, 10, 10)]
        public async Task GetDifferenceToLeaderTest(int result, int time, int leaderTime)
        {
            var mockSensorDao = new Mock<ISensorDao>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(3);
            mockTimeDataDao.Setup(tdd => tdd.GetRankingForSensor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<TimeData>
                           {
                               new TimeData {SkierId = 99}
                           });
            mockTimeDataDao.Setup(tdd => tdd.FindByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new TimeData {Time = leaderTime});

            Assert.AreEqual(TimeSpan.FromMilliseconds(result),
                            await new RaceStatService(null, mockTimeDataDao.Object, mockSensorDao.Object, null)
                                .GetDifferenceToLeader(new TimeData
                                                           {SkierId = 1, RaceId = 1, SensorId = 1, Time = time}));
        }

        [Test]
        public async Task GetDifferenceToLeaderTestNoLeader()
        {
            var mockSensorDao = new Mock<ISensorDao>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(3);
            mockTimeDataDao.Setup(tdd => tdd.GetRankingForSensor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<TimeData>());

            Assert.AreEqual(TimeSpan.Zero,
                            await new RaceStatService(null, mockTimeDataDao.Object, mockSensorDao.Object, null)
                                .GetDifferenceToLeader(new TimeData
                                                           {SkierId = 1, RaceId = 1, SensorId = 1, Time = 0}));
        }

        [Test]
        public async Task GetDifferenceToLeaderTestNoLeaderTime()
        {
            var mockSensorDao = new Mock<ISensorDao>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(3);
            mockTimeDataDao.Setup(tdd => tdd.GetRankingForSensor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<TimeData>
                           {
                               new TimeData {SkierId = 99}
                           });
            mockTimeDataDao.Setup(tdd => tdd.FindByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync((TimeData?) null);

            Assert.AreEqual(null, await new RaceStatService(null, mockTimeDataDao.Object, mockSensorDao.Object, null)
                                .GetDifferenceToLeader(new TimeData
                                                           {SkierId = 1, RaceId = 1, SensorId = 1, Time = 0}));
        }

        [Test]
        public async Task GetDifferenceToLeaderTestIsLeader()
        {
            var mockSensorDao = new Mock<ISensorDao>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(3);
            mockTimeDataDao.Setup(tdd => tdd.GetRankingForSensor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<TimeData>
                           {
                               new TimeData {SkierId = 1},
                               new TimeData {SkierId = 99}
                           });
            mockTimeDataDao.Setup(tdd => tdd.FindByIdAsync(99, It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new TimeData {Time = 10});

            Assert.AreEqual(TimeSpan.FromMilliseconds(-5),
                            await new RaceStatService(null, mockTimeDataDao.Object, mockSensorDao.Object, null)
                                .GetDifferenceToLeader(new TimeData
                                                           {SkierId = 1, RaceId = 1, SensorId = 1, Time = 5}));
        }


        [Test]
        public async Task GetTimeDataForSkierWithDifference()
        {
            var mockSensorDao = new Mock<ISensorDao>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(3);
            mockTimeDataDao.Setup(tdd => tdd.GetTimeDataForStartList(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(
                               new List<TimeData>
                               {
                                   new TimeData {SkierId = 1, SensorId = 1, Time = 6},
                                   new TimeData {SkierId = 1, SensorId = 2, Time = 9}
                               });
            mockTimeDataDao.Setup(tdd => tdd
                                      .GetRankingForSensor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<TimeData>
                           {
                               new TimeData {SkierId = 99, SensorId = 1},
                               new TimeData {SkierId = 1, SensorId = 1}
                           });
            mockTimeDataDao.Setup(tdd => tdd.FindByIdAsync(99, It.IsAny<int>(), 1))
                           .ReturnsAsync(new TimeData {Time = 5, SkierId = 1});
            mockTimeDataDao.Setup(tdd => tdd.FindByIdAsync(99, It.IsAny<int>(), 2))
                           .ReturnsAsync(new TimeData {Time = 10, SkierId = 1});


            var result = (await new RaceStatService(null, mockTimeDataDao.Object, mockSensorDao.Object, null)
                .GetTimeDataForSkierWithDifference(1, 1)).ToList();
            Assert.AreEqual(TimeSpan.FromMilliseconds(1), result[0].DifferenceToLeader);
            Assert.AreEqual(TimeSpan.FromMilliseconds(-1), result[1].DifferenceToLeader);
        }


        private static object[] _getStartTimeForSkierTestSource =
        {
            new object[] {null, null, null},
            new object[] {new Sensor {Id = 5}, null, null},
            new object[]
            {
                new Sensor {Id = 5}, new TimeData
                {
                    SkierEvent = new SkierEvent
                        {RaceData = new RaceData {EventDateTime = DateTime.Today}}
                },
                DateTime.Today
            }
        };

        [Test]
        [TestCaseSource(nameof(_getStartTimeForSkierTestSource))]
        public async Task GetStartTimeForSkierTest(Sensor sensor, TimeData timeData, DateTime? expected)
        {
            var mockSensorDao = new Mock<ISensorDao>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            mockSensorDao.Setup(sd => sd.GetSensorForSensorNumber(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(sensor);
            mockTimeDataDao.Setup(tdd => tdd.FindByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(timeData);

            Assert.AreEqual(
                expected,
                await new RaceStatService(null, mockTimeDataDao.Object, mockSensorDao.Object, null)
                    .GetStartTimeForSkier(1, 1));
        }


        [Test]
        public async Task GetRankingForRaceTest()
        {
            var mockSensorDao = new Mock<ISensorDao>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();
            var mockStartListDao = new Mock<IStartListDao>();
            mockSensorDao.Setup(sd => sd.GetLastSensorNumber(It.IsAny<int>())).ReturnsAsync(3);
            mockTimeDataDao.Setup(sld => sld.GetRankingForSensor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<TimeData>
                           {
                               new TimeData {Time = 0, StartList = new StartList {SkierId = 1}},
                               new TimeData {Time = 3, StartList = new StartList {SkierId = 2}},
                               new TimeData {Time = 3, StartList = new StartList {SkierId = 3}},
                               new TimeData {Time = 4, StartList = new StartList {SkierId = 4}},
                           });
            mockStartListDao.Setup(sld => sld.GetDisqualifiedSkierForRace(It.IsAny<int>())).ReturnsAsync(
                new List<StartList>
                {
                    new StartList {SkierId = 5}
                });

            var ranking =
                (await new RaceStatService(mockStartListDao.Object, mockTimeDataDao.Object, mockSensorDao.Object, null)
                    .GetRankingForRace(1)).ToList();

            Assert.AreEqual(1, ranking[0].Position);
            Assert.AreEqual(1, ranking[0].StartList.SkierId);

            Assert.AreEqual(2, ranking[1].Position);
            Assert.AreEqual(2, ranking[1].StartList.SkierId);

            Assert.AreEqual(2, ranking[2].Position);
            Assert.AreEqual(3, ranking[2].StartList.SkierId);

            Assert.AreEqual(4, ranking[3].Position);
            Assert.AreEqual(4, ranking[3].StartList.SkierId);

            Assert.AreEqual(5, ranking[4].StartList.SkierId);
            Assert.IsNull(ranking[4].Position);
        }
    }
}