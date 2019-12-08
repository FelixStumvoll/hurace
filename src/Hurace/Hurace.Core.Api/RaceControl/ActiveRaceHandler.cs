using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Api.RaceControl
{
    public class ActiveRaceHandler : IActiveRaceHandler
    {
        private List<IRaceControlService> _activeRaces = new List<IRaceControlService>();

        private readonly IRaceDao _raceDao;
        private readonly IRaceEventDao _raceEventDao;
        private readonly IRaceDataDao _raceDataDao;

        public static ActiveRaceHandler Instance { get; private set; }

        private ActiveRaceHandler()
        {
            var serviceProvider = ServiceProvider.Instance;
            _raceDao = serviceProvider.ResolveService<IRaceDao>();
            _raceDataDao = serviceProvider.ResolveService<IRaceDataDao>();
            _raceEventDao = serviceProvider.ResolveService<IRaceEventDao>();
        }

        public static async Task InitializeActiveRaceHandler()
        {
            Instance = new ActiveRaceHandler();
            var provider = ServiceProvider.Instance;
            Instance._activeRaces.AddRange((await Instance._raceDao.GetActiveRaces()).Select(r =>
            {
                var rcs = provider.ResolveService<IRaceControlService>();
                rcs.RaceId = r.Id;
                return rcs;
            }));
        }
        
        public async Task<IRaceControlService> StartRace(int raceId)
        {
            await ChangeRaceState(raceId, Constants.RaceState.Running);
            var service = ServiceProvider.Instance.ResolveService<IRaceControlService>();
            service.RaceId = raceId;
            _activeRaces.Add(service);
            return service;
        }

        public IRaceControlService this[int raceId] =>  _activeRaces.SingleOrDefault(r => r.RaceId == raceId);
        
        private async Task ChangeRaceState(int raceId, Constants.RaceState state)
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
            raceData.Id = await _raceDataDao.InsertGetIdAsync(raceData);
            await _raceEventDao.InsertAsync(new RaceEvent
            {
                RaceDataId = raceData.Id
            });
        }

        public async Task EndRace(int raceId)
        {
            await ChangeRaceState(raceId, Constants.RaceState.Finished);
            _activeRaces = _activeRaces.Where(r => r.RaceId != raceId).ToList();
        }
    }
}