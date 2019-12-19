using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Api.ActiveRaceControlService.Service;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using RaceState = Hurace.Dal.Domain.Enums.RaceState;

namespace Hurace.Core.Api.ActiveRaceControlService.Resolver
{
    public class ActiveRaceResolver : IActiveRaceResolver
    {
        private List<IActiveRaceControlService> _activeRaces = new List<IActiveRaceControlService>();

        private readonly IRaceDao _raceDao;
        private readonly IRaceEventDao _raceEventDao;
        private readonly IRaceDataDao _raceDataDao;

        public static ActiveRaceResolver Instance { get; private set; }

        private ActiveRaceResolver()
        {
            var serviceProvider = ServiceProvider.Instance;
            _raceDao = serviceProvider.ResolveService<IRaceDao>();
            _raceDataDao = serviceProvider.ResolveService<IRaceDataDao>();
            _raceEventDao = serviceProvider.ResolveService<IRaceEventDao>();
        }

        public static async Task<bool> InitializeActiveRaceHandler()
        {
            Instance = new ActiveRaceResolver();
            var provider = ServiceProvider.Instance;

            foreach (var race in await Instance._raceDao.GetActiveRaces())
            {
                var rcs = provider.ResolveService<IActiveRaceControlService>();
                rcs.RaceId = race.Id;
                await rcs.InitializeAsync();
                Instance._activeRaces.Add(rcs);
            }

            return true;
        }

        public async Task<IActiveRaceControlService> StartRace(int raceId)
        {
            await RaceClockProvider.Instance.GetRaceClock();
            await ChangeRaceState(raceId, RaceState.Running);
            var service = ServiceProvider.Instance.ResolveService<IActiveRaceControlService>();
            service.RaceId = raceId;
            await service.InitializeAsync();
            _activeRaces.Add(service);
            return service;
        }

        public IActiveRaceControlService this[int raceId] => _activeRaces
            .SingleOrDefault(r => r.RaceId == raceId);

        private async Task ChangeRaceState(int raceId, RaceState state)
        {
            var race = await _raceDao.FindByIdAsync(raceId);
            race.RaceStateId = (int) state;
            await _raceDao.UpdateAsync(race);
            var raceData = new RaceData
            {
                EventTypeId = (int) state,
                RaceId = race.Id,
                EventDateTime = DateTime.Now
            };
            var raceDataId = await _raceDataDao.InsertGetIdAsync(raceData);
            if(!raceDataId.HasValue) return;
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