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


        private static readonly Lazy<ActiveRaceHandler> LazyInstance =
            new Lazy<ActiveRaceHandler>(() => new ActiveRaceHandler());

        public static ActiveRaceHandler Instance => LazyInstance.Value;

        private ActiveRaceHandler()
        {
            var serviceProvider = ServiceProvider.Instance;

            _raceDao = serviceProvider.ResolveService<IRaceDao>();
            _raceDataDao = serviceProvider.ResolveService<IRaceDataDao>();
            _raceEventDao = serviceProvider.ResolveService<IRaceEventDao>();
        }


        public async Task<IRaceControlService> StartRace(int raceId)
        {
            var race = await _raceDao.FindByIdAsync(raceId);
            race.RaceStateId = (int) Constants.RaceState.Running;
            await _raceDao.UpdateAsync(race);
            var raceData = new RaceData
            {
                EventTypeId = (int) Constants.RaceEvent.Started,
                RaceId = race.Id,
                EventDateTime = DateTime.Now
            };
            raceData.Id = await _raceDataDao.InsertGetIdAsync(raceData);
            await _raceEventDao.InsertAsync(new RaceEvent
            {
                RaceDataId = raceData.Id
            });

            var service = ServiceProvider.Instance.ResolveService<IRaceControlService>();
            service.RaceId = raceId;
            _activeRaces.Add(service);
            return service;
        }

        public IRaceControlService GetRaceControlServiceForRace(int raceId) =>
            _activeRaces.SingleOrDefault(r => r.RaceId == raceId);

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