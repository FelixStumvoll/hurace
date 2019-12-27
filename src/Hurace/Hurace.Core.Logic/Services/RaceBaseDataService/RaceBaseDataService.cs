﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Services.RaceStartListService;
using Hurace.Core.Logic.Util;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Logic.Services.RaceBaseDataService
{
    public class RaceBaseDataService : IRaceBaseDataService
    {
        private readonly IRaceDao _raceDao;
        private readonly ISensorDao _sensorDao;
        private readonly ITimeDataDao _timeDataDao;
        private readonly IRaceStartListService _startListService;
        private readonly IGenderDao _genderDao;
        private readonly ILocationDao _locationDao;

        public RaceBaseDataService(IRaceDao raceDao, ISensorDao sensorDao, ITimeDataDao timeDataDao, IRaceStartListService startListService, IGenderDao genderDao, ILocationDao locationDao)
        {
            _raceDao = raceDao;
            _sensorDao = sensorDao;
            _timeDataDao = timeDataDao;
            _startListService = startListService;
            _genderDao = genderDao;
            _locationDao = locationDao;
        }

        public Task<Race?> GetRaceById(int raceId) => _raceDao.FindByIdAsync(raceId);
        
        public async Task<RaceModificationResult> InsertOrUpdateRace(Race race, int sensorCount)
        {
            try
            {
                using var scope = ScopeBuilder.BuildTransactionScope();
                if (race.Id == -1)
                {
                    var raceId = await _raceDao.InsertGetIdAsync(race);
                    if (raceId.HasValue) race.Id = raceId.Value;
                    else return RaceModificationResult.Err;
                }
                else if (!await IsUpdateInvalid(race)) await _raceDao.UpdateAsync(race);
                else return RaceModificationResult.StartListDefined;

                var sensors = (await _sensorDao.FindAllSensorsForRace(race.Id)).ToList();

                if (sensors.Count > sensorCount)
                    foreach (var s in sensors.Where(s => s.SensorNumber >= sensorCount))
                        await _sensorDao.DeleteAsync(s.Id);
                else
                    for (var i = sensors.Count; i < sensorCount; i++)
                        await _sensorDao.InsertAsync(new Sensor {RaceId = race.Id, SensorNumber = i});

                scope.Complete();
                return RaceModificationResult.Ok;
            }
            catch (Exception)
            {
                return RaceModificationResult.Err;
            }
        }

        private async Task<bool> IsUpdateInvalid(Race race)
        {
            var ogRace = await _raceDao.FindByIdAsync(race.Id);
            if (ogRace == null) return true;
            var slDefined = (await _startListService.IsStartListDefined(race.Id)) ?? false;
            return slDefined && (ogRace.DisciplineId != race.DisciplineId || ogRace.GenderId != race.GenderId);
        }

        public async Task<int?> GetSensorCount(int raceId) => (await _sensorDao.FindAllSensorsForRace(raceId))?.Count();

        public async Task<bool> RemoveRace(Race race)
        {
            if (await _raceDao.FindByIdAsync(race.Id) == null ||
                (await _startListService.IsStartListDefined(race.Id) ?? false) ||
                await _timeDataDao.CountTimeDataForRace(race.Id) != 0)
                return false;
            await _sensorDao.DeleteAllSensorsForRace(race.Id);
            await _raceDao.DeleteAsync(race.Id);
            return true;
        }

        public Task<IEnumerable<Gender>> GetGenders() => _genderDao.FindAllAsync();

        public Task<IEnumerable<Location>> GetLocations() => _locationDao.FindAllAsync();
        
        public Task<IEnumerable<Discipline>> GetDisciplinesForLocation(int locationId) =>
            _locationDao.GetPossibleDisciplinesForLocation(locationId);
    }
}