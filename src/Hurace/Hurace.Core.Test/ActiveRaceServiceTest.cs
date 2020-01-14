using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Services.ActiveRaceService;
using Hurace.Core.Logic.Services.RaceStatService;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Moq;
using NUnit.Framework;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class ActiveRaceServiceTest
    {
        private static object[] _getSplitTimesForCurrentSkierTest =
        {
            new object[]
            {
                null, null
            },
            new object[]
            {
                new StartList(), new List<TimeDifference>
                {
                    new TimeDifference(new TimeData(), TimeSpan.Zero),
                    new TimeDifference(new TimeData(), TimeSpan.Zero)
                },
            },
        };

        [Test]
        [TestCaseSource(nameof(_getSplitTimesForCurrentSkierTest))]
        public async Task GetSplitTimesForCurrentSkierTest(StartList? currentSkier, IEnumerable<TimeDifference>? result)
        {
            var mockStartListDao = new Mock<IStartListDao>();
            var mockStatService = new Mock<IRaceStatService>();

            mockStartListDao.Setup(sld => sld.GetCurrentSkierForRace(It.IsAny<int>())).ReturnsAsync(currentSkier);
            mockStatService.Setup(ss => ss.GetTimeDataForSkierWithDifference(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(result);

            var methodResult = await new ActiveRaceService(mockStartListDao.Object, null, mockStatService.Object, null)
                .GetSplitTimesForCurrentSkier(100);

            Assert.AreEqual(result, methodResult);
        }

        private static object[] _getPossiblePositionForCurrentSkierTestSource =
        {
            new object[]
            {
                null, null, null, null, null
            },
            new object[]
            {
                new StartList(), new List<TimeData>
                {
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 0},
                    },
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 1},
                    },
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 2},
                    },
                },
                null, null, 1
            },
            new object[]
            {
                new StartList(), new List<TimeData>
                {
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 0},
                    },
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 1},
                    },
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 2},
                    },
                },
                TimeSpan.FromMilliseconds(50), new List<RaceRanking>
                {
                    new RaceRanking(new StartList(), 1500,1,0),
                    new RaceRanking(new StartList(), 1600,2,100)
                }, 2
            },
            new object[]
            {
                new StartList(), new List<TimeData>
                {
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 0},
                    },
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 1},
                    },
                    new TimeData
                    {
                        Sensor = new Sensor {SensorNumber = 2},
                    },
                },
                TimeSpan.FromMilliseconds(-10), new List<RaceRanking>
                {
                    new RaceRanking(new StartList(), 1500,1,0),
                    new RaceRanking(new StartList(), 1600,2,100)
                }, 1
            },
        };

        [Test]
        [TestCaseSource(nameof(_getPossiblePositionForCurrentSkierTestSource))]
        public async Task GetPossiblePositionForCurrentSkierTest(StartList? currentSkier,
            List<TimeData> timeDataList, TimeSpan? differenceToLeader, IEnumerable<RaceRanking>? ranking,
            int? position)
        {
            var mockStartListDao = new Mock<IStartListDao>();
            var mockStatService = new Mock<IRaceStatService>();
            var mockTimeDataDao = new Mock<ITimeDataDao>();

            mockStartListDao.Setup(sld => sld.GetCurrentSkierForRace(It.IsAny<int>())).ReturnsAsync(currentSkier);
            mockTimeDataDao.Setup(sld => sld.GetTimeDataForStartList(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(timeDataList);
            mockStatService.Setup(ss => ss.GetDifferenceToLeader(timeDataList == null ? It.IsAny<TimeData>() : timeDataList.Last()))
                           .ReturnsAsync(differenceToLeader);

            mockStatService.Setup(ss => ss.GetFinishedSkierRanking(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(ranking);

            var methodResult =
                await new ActiveRaceService(mockStartListDao.Object, mockTimeDataDao.Object, mockStatService.Object,
                                            null).GetPossiblePositionForCurrentSkier(55);

            Assert.AreEqual(position, methodResult);
        }
    }
}