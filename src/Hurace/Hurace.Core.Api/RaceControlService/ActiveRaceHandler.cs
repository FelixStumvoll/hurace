using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Api.RaceControlService
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

        public static async Task<bool> InitializeActiveRaceHandler()
        {
            try
            {
                Instance = new ActiveRaceHandler();
                var provider = ServiceProvider.Instance;

                foreach (var race in await Instance._raceDao.GetActiveRaces())
                {
                    var rcs = provider.ResolveService<IRaceControlService>();
                    await rcs.InitializeAsync();
                    rcs.RaceId = race.Id;
                    Instance._activeRaces.Add(rcs);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IRaceControlService?> StartRace(int raceId)
        {
            try
            {
                await RaceClockProvider.Instance.GetRaceClock();
                await ChangeRaceState(raceId, Constants.RaceState.Running);
                var service = ServiceProvider.Instance.ResolveService<IRaceControlService>();
                service.RaceId = raceId;
                await service.InitializeAsync();
                _activeRaces.Add(service);
                return service;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IRaceControlService this[int raceId] => _activeRaces.SingleOrDefault(r => r.RaceId == raceId);

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

        public async Task<bool> EndRace(int raceId)
        {
            try
            {
                await ChangeRaceState(raceId, Constants.RaceState.Finished);
                _activeRaces = _activeRaces.Where(r => r.RaceId != raceId).ToList();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}