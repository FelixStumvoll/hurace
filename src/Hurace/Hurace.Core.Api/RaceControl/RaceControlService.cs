using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Api.RaceControl.Events;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using RaceEvent = Hurace.Dal.Domain.RaceEvent;
using SkierEvent = Hurace.Dal.Domain.SkierEvent;

namespace Hurace.Core.Api.RaceControl
{
    public class RaceControlService : IRaceControlService
    {
        public event Action<TimeData> OnTimeData;
        public event Action<StartList> OnSkierStarted;
        public event Action<StartList> OnSkierFinished;
        public event Action<StartList> OnSkierCanceled;
        public event Action<StartList> OnCurrentSkierDisqualified;
        public event Action<StartList> OnLateDisqualification;
        public event Action<TimeData> OnSplitTime;
        public event Action OnRaceCanceled;
        public event Action OnRaceFinished;

        private readonly IRaceDao _raceDao;
        private readonly IStartListDao _startListDao;
        private readonly IRaceEventDao _raceEventDao;
        private readonly IRaceDataDao _raceDataDao;
        private readonly ISkierEventDao _skierEventDao;
        private ITimeDataDao _timeDataDao;
        public int RaceId { get; set; }

        public RaceControlService(IRaceDao raceDao, IStartListDao startListDao, IRaceEventDao raceEventDao,
            IRaceDataDao raceDataDao, ISkierEventDao skierEventDao, ITimeDataDao timeDataDao)
        {
            _raceDao = raceDao;
            _startListDao = startListDao;
            _raceEventDao = raceEventDao;
            _raceDataDao = raceDataDao;
            _skierEventDao = skierEventDao;
            _timeDataDao = timeDataDao;
        }

        public async Task EnableRaceForSkier()
        {
            var startList = await _startListDao.GetNextSkierForRace(RaceId);
            await UpdateStartListState(startList, Constants.StartState.Running);
            OnSkierStarted?.Invoke(startList);
        }

        private async Task UpdateStartListState(StartList startList, Constants.StartState state)
        {
            startList.StartStateId = (int) state;
            await _startListDao.UpdateAsync(startList);
            var raceData = new RaceData
            {
                EventTypeId = (int) state,
                RaceId = startList.RaceId,
                EventDateTime = DateTime.Now
            };

            raceData.Id = await _raceDataDao.InsertGetIdAsync(raceData);
            await _skierEventDao.InsertAsync(new SkierEvent
            {
                RaceId = startList.RaceId,
                SkierId = startList.SkierId,
                RaceDataId = raceData.Id
            });
        }

        public async Task<StartList> GetCurrentSkier() => await _startListDao.GetCurrentSkierForRace(RaceId);

        public async Task CancelSkier(int skierId)
        {
            var startList = await _startListDao.GetSkierForRace(skierId, RaceId);
            await UpdateStartListState(startList, Constants.StartState.Canceled);
            OnSkierCanceled?.Invoke(startList);
        }

        public async Task<IEnumerable<StartList>> GetRemainingStartList() =>
            (await _startListDao.GetStartListForRace(RaceId)).Where(
                sl => sl.StartStateId == (int) Constants.StartState.Upcoming);

        public void CancelRace()
        {
        }
    }
}