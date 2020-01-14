using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Service;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using RaceState = Hurace.Dal.Domain.Enums.RaceState;

namespace Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver
{
    public class ActiveRaceResolver : IActiveRaceResolver
    {
        private List<IActiveRaceControlService> _activeRaces = new List<IActiveRaceControlService>();
    
        private readonly IRaceDao _raceDao;
        private readonly IRaceEventDao _raceEventDao;
        private readonly IRaceDataDao _raceDataDao;
        private readonly Func<int, IActiveRaceControlService> _activeRaceControlFactory;
    
        public ActiveRaceResolver(Func<int,IActiveRaceControlService> activeRaceControlFactory, IRaceDao raceDao,
            IRaceEventDao raceEventDao, IRaceDataDao raceDataDao)
        {
            _activeRaceControlFactory = activeRaceControlFactory;
            _raceDao = raceDao;
            _raceEventDao = raceEventDao;
            _raceDataDao = raceDataDao;
        }
    
        public async Task InitializeActiveRaceResolver()
        {
            foreach (var race in await _raceDao.GetActiveRaces())
            {
                var rcs = _activeRaceControlFactory(race.Id);
                if (rcs == null) continue;
                await rcs.InitializeAsync();
                _activeRaces.Add(rcs);
            }
        }
    
        public async Task<IActiveRaceControlService?> StartRace(int raceId)
        {
            var service = _activeRaceControlFactory(raceId);
            if (service == null) return null;
            await service.InitializeAsync();
            if (!await service.StartRace()) return null;
            _activeRaces.Add(service);

            void RemoveRace(IActiveRaceControlService s)
            {
                _activeRaces.Remove(s);
            }

            service.OnRaceFinished += _ => RemoveRace(service);
            service.OnRaceCancelled += _ => RemoveRace(service);
            return service;
        }
    
        public IActiveRaceControlService this[int raceId] => _activeRaces
            .SingleOrDefault(r => r.RaceId == raceId);
    
        private async Task ChangeRaceState(int raceId, RaceState state)
        {
            var race = await _raceDao.FindByIdAsync(raceId);
            if (race == null) return;
            race.RaceStateId = (int) state;
            await _raceDao.UpdateAsync(race);
            var raceData = new RaceData
            {
                EventTypeId = (int) state,
                RaceId = race.Id,
                EventDateTime = DateTime.Now
            };
            var raceDataId = await _raceDataDao.InsertGetIdAsync(raceData);
            if (!raceDataId.HasValue) return;
            raceData.Id = raceDataId.Value;
            await _raceEventDao.InsertAsync(new RaceEvent
            {
                RaceDataId = raceData.Id
            });
        }
    
        public async Task<bool> EndRace(int raceId)
        {
            await ChangeRaceState(raceId, RaceState.Finished);
            _activeRaces = _activeRaces.Where(r => r.RaceId != raceId).ToList();
            return true;
        }
    }
}