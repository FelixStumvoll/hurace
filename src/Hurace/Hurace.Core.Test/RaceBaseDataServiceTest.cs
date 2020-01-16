using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public class RaceBaseDataServiceTest
    {
        private static RaceBaseDataService CreateBaseDataService(IRaceDao raceDao = null, ISensorDao sensorDao = null,
            ITimeDataDao timeDataDao = null, IRaceStartListService startListService = null, IGenderDao genderDao = null,
            ILocationDao locationDao = null, IDisciplineDao disciplineDao = null) =>
            new RaceBaseDataService(raceDao, sensorDao, timeDataDao, startListService, genderDao, locationDao,
                                    disciplineDao);

        [Test]
        public async Task InsertOrUpdateRaceNoIdTest()
        {
            var race = new Race {Id = -1};
            var raceDaoMock = new Mock<IRaceDao>();
            raceDaoMock.Setup(rd => rd.InsertGetIdAsync(race)).ReturnsAsync((int?) null);

            var baseDataService = CreateBaseDataService(raceDaoMock.Object);
            Assert.AreEqual(RaceModificationResult.Err, await baseDataService.InsertOrUpdateRace(race, 4));
        }

        [Test]
        public async Task InsertOrUpdateRaceNoRaceFoundTest()
        {
            var race = new Race {Id = 25};
            var raceDaoMock = new Mock<IRaceDao>();
            raceDaoMock.Setup(rd => rd.FindByIdAsync(25)).ReturnsAsync((Race?) null);
            var baseDataService = CreateBaseDataService(raceDaoMock.Object);
            Assert.AreEqual(RaceModificationResult.StartListDefined, await baseDataService.InsertOrUpdateRace(race, 4));
        }

        [Test]
        [TestCase(25, 0, 5, 4, true, 4, 5, RaceModificationResult.StartListDefined)]
        [TestCase(25, 0, 4, 5, true, 4, 3, RaceModificationResult.StartListDefined)]
        [TestCase(25, 0, 4, 5, false, 4, 4, RaceModificationResult.Ok)]
        [TestCase(25, 0, 4, 5, false, 5, 4, RaceModificationResult.Ok)]
        [TestCase(-1, 25, 4, 5, false, 0, 4, RaceModificationResult.Ok)]
        [TestCase(-1, null, 4, 5, false, 0, 4, RaceModificationResult.Err)]
        public async Task InsertOrUpdateRaceStartListDefinedTest(int raceId, int? idFromDb, int disciplineId,
            int genderId, bool startListDefined,
            int sensorsDefined, int sensorsRequested, RaceModificationResult result)
        {
            var race = new Race {Id = raceId, DisciplineId = 5, GenderId = 5};
            var raceDaoMock = new Mock<IRaceDao>();
            var startListServiceMock = new Mock<IRaceStartListService>();
            var sensorDaoMock = new Mock<ISensorDao>();
            raceDaoMock.Setup(rd => rd.FindByIdAsync(It.IsAny<int>()))
                       .ReturnsAsync(new Race {GenderId = genderId, DisciplineId = disciplineId});
            raceDaoMock.Setup(rd => rd.InsertGetIdAsync(It.IsAny<Race>())).ReturnsAsync(idFromDb);
            startListServiceMock.Setup(sls => sls.IsStartListDefined(race.Id)).ReturnsAsync(startListDefined);
            sensorDaoMock.Setup(sd => sd.FindAllSensorsForRace(race.Id)).ReturnsAsync(() =>
            {
                var sensorList = new List<Sensor>();
                for (var i = 0; i < sensorsDefined; i++) sensorList.Add(new Sensor {Id = i, SensorNumber = i});
                return sensorList;
            });
            var sensorsDeleted = 0;
            var sensorsAdded = 0;
            sensorDaoMock.Setup(sd => sd.DeleteAsync(It.IsAny<int>())).Callback(() => sensorsDeleted++);
            sensorDaoMock.Setup(sd => sd.InsertAsync(It.IsAny<Sensor>())).Callback(() => sensorsAdded++);
            var baseDataService =
                CreateBaseDataService(raceDaoMock.Object, startListService: startListServiceMock.Object,
                                      sensorDao: sensorDaoMock.Object);
            Assert.AreEqual(result, await baseDataService.InsertOrUpdateRace(race, sensorsRequested));

            if (result != RaceModificationResult.Ok) return;

            var expected = 0;
            int actual;

            if (sensorsDefined > sensorsRequested)
            {
                expected = sensorsDefined - sensorsRequested;
                actual = sensorsDeleted;
            }
            else if (sensorsDefined < sensorsRequested)
            {
                expected = sensorsRequested - sensorsDefined;
                actual = sensorsAdded;
            }
            else
            {
                actual = sensorsAdded + sensorsDeleted;
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task InsertOrUpdateRaceExceptionTest()
        {
            var raceDaoMock = new Mock<IRaceDao>();
            raceDaoMock.Setup(rd => rd.InsertGetIdAsync(It.IsAny<Race>())).Throws(new Exception());
            var raceBaseData = CreateBaseDataService(raceDaoMock.Object);

            Assert.AreEqual(RaceModificationResult.Err, await raceBaseData.InsertOrUpdateRace(new Race {Id = -1}, 0));
        }

        [Test]
        public async Task TestSensorCount()
        {
            var sensorDaoMock = new Mock<ISensorDao>();
            sensorDaoMock.Setup(sd => sd.FindAllSensorsForRace(It.IsAny<int>())).ReturnsAsync(() => new List<Sensor>
            {
                new Sensor(),
                new Sensor()
            });

            var raceBaseData = CreateBaseDataService(sensorDao: sensorDaoMock.Object);
            Assert.AreEqual(2, await raceBaseData.GetSensorCount(1));
        }


        private static object[] _removeRaceTestSource =
        {
            new object[] {null, true, 0, false },
            new object[] {new Race(), true, 0, false },
            new object[] {new Race(), false, 1, false },
            new object[] {new Race(), false, 0, true }
        };

        [Test]
        [TestCaseSource(nameof(_removeRaceTestSource))]
        public async Task RemoveRaceTest(Race? race, bool startListDefined, int tdCount, bool result)
        {
            var mockRaceDao = new Mock<IRaceDao>();
            var mockStartListService = new Mock<IRaceStartListService>();
            var mockTimedataDao = new Mock<ITimeDataDao>();
            var mockSensorDao = new Mock<ISensorDao>();

            mockRaceDao.Setup(rd => rd.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(race);
            mockRaceDao.Setup(rd => rd.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

            mockStartListService.Setup(sl => sl.IsStartListDefined(It.IsAny<int>())).ReturnsAsync(startListDefined);
            mockTimedataDao.Setup(td => td.CountTimeDataForRace(It.IsAny<int>())).ReturnsAsync(tdCount);

            Assert.AreEqual(
                result,
                await new RaceBaseDataService(mockRaceDao.Object, mockSensorDao.Object, mockTimedataDao.Object,
                                        mockStartListService.Object, null, null, null).RemoveRace(new Race{Id = 100}));
            
        }
    }
}