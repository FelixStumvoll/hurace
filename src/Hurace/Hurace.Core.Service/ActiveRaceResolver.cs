using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Dal.Interface;

namespace Hurace.Core.Service
{
    public class ActiveRaceResolver : IActiveRaceResolver
    {
        private readonly List<IActiveRaceControlService> _activeRaces = new List<IActiveRaceControlService>();

        private readonly IRaceDao _raceDao;
        private readonly Func<int, IActiveRaceControlService> _activeRaceControlFactory;

        public ActiveRaceResolver(Func<int, IActiveRaceControlService> activeRaceControlFactory, IRaceDao raceDao)
        {
            _activeRaceControlFactory = activeRaceControlFactory;
            _raceDao = raceDao;
        }

        public async Task InitializeActiveRaceResolver()
        {
            foreach (var race in await _raceDao.GetActiveRaces())
            {
                var rcs = _activeRaceControlFactory(race.Id);
                await rcs.InitializeAsync();
                _activeRaces.Add(rcs);
                RegisterRemoveHandler(rcs);
            }
        }

        private void RegisterRemoveHandler(IActiveRaceControlService service)
        {
            void RemoveRace(IActiveRaceControlService s) => _activeRaces.Remove(s);

            service.OnRaceFinished += _ => RemoveRace(service);
            service.OnRaceCancelled += _ => RemoveRace(service);
        }

        public async Task<IActiveRaceControlService?> StartRace(int raceId)
        {
            var service = _activeRaceControlFactory(raceId);
            if (service == null) return null;
            await service.InitializeAsync();
            if (!await service.StartRace()) return null;
            _activeRaces.Add(service);
            RegisterRemoveHandler(service);

            return service;
        }

        [ExcludeFromCodeCoverage]
        public IActiveRaceControlService this[int raceId] => _activeRaces
            .SingleOrDefault(r => r.RaceId == raceId);
    }
}