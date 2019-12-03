using System;
using System.Threading.Tasks;
using Hurace.Core.Api.RaceControl.Events;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using RaceEvent = Hurace.Dal.Domain.RaceEvent;
using SkierEvent = Hurace.Dal.Domain.SkierEvent;

namespace Hurace.Core.Api.RaceControl
{
    public class RaceControl
    {
        public event Action<TimeData> OnTimeData;
        public event Action<Event> OnEvent;
        private IRaceDao _raceDao;
        private IStartListDao _startListDao;
        private IRaceEventDao _raceEventDao;
        private IRaceDataDao _raceDataDao;
        private ISkierEventDao _skierEventDao;
        private ITimeDataDao _timeDataDao;

        public RaceControl(IRaceDao raceDao, IStartListDao startListDao, IRaceEventDao raceEventDao,
            IRaceDataDao raceDataDao, ISkierEventDao skierEventDao, ITimeDataDao timeDataDao)
        {
            _raceDao = raceDao;
            _startListDao = startListDao;
            _raceEventDao = raceEventDao;
            _raceDataDao = raceDataDao;
            _skierEventDao = skierEventDao;
            _timeDataDao = timeDataDao;
        }

        public async Task<bool> StartRace(Race race)
        {
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

            OnEvent?.Invoke(new Events.RaceEvent
            {
                Race = race,
                RaceData = raceData
            });

            return await _raceDao.UpdateAsync(race);
        }

        public async Task EnableRaceForSkier(Race race)
        {
            var skier = await _startListDao.GetNextSkierForRace(race.Id);
            skier.StartStateId = (int) Constants.StartState.Running;
            await _startListDao.UpdateAsync(skier);
            var raceData = new RaceData
            {
                EventTypeId = (int) Constants.StartState.Running,
                RaceId = race.Id,
                EventDateTime = DateTime.Now
            };

            raceData.Id = await _raceDataDao.InsertGetIdAsync(raceData);
            await _skierEventDao.InsertAsync(new SkierEvent
            {
                RaceId = race.Id,
                SkierId = skier.SkierId,
                RaceDataId = raceData.Id
            });
        }

        public void CancelSkier(Skier skier)
        {
        }

        public void CancelRace()
        {
        }
    }
}