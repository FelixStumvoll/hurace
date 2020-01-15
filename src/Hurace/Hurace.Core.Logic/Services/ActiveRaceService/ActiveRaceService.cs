using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Services.RaceStatService;
using Hurace.Dal.Dao;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.Core.Logic.Services.ActiveRaceService
{
    public class ActiveRaceService : IActiveRaceService
    {
        private readonly IStartListDao _startListDao;
        private readonly ITimeDataDao _timeDataDao;
        private readonly IRaceStatService _statService;
        private readonly IRaceDao _raceDao;

        public ActiveRaceService(IStartListDao startListDao, ITimeDataDao timeDataDao, IRaceStatService statService, IRaceDao raceDao)
        {
            _startListDao = startListDao;
            _timeDataDao = timeDataDao;
            _statService = statService;
            _raceDao = raceDao;
        }

        [ExcludeFromCodeCoverage]
        public Task<StartList?> GetCurrentSkier(int raceId) => _startListDao.GetCurrentSkierForRace(raceId);
        
        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Race>> GetActiveRaces() => _raceDao.GetActiveRaces();

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<StartList>?> GetRemainingStartList(int raceId) =>
            (await _startListDao.GetRemainingStartListForRace(raceId)).OrderBy(sl => sl.StartNumber);

        public async Task<int?> GetPossiblePositionForCurrentSkier(int raceId)
        {
            var current = await GetCurrentSkier(raceId);
            if (current == null) return null;
            var lastTimeData =
                (await _timeDataDao.GetTimeDataForStartList(current.SkierId, current.RaceId))
                .OrderByDescending(td => td?.Sensor?.SensorNumber)
                .First();

            var diff = await _statService.GetDifferenceToLeader(lastTimeData);
            if (diff == null) return 1;

            var ranking = await _statService.GetFinishedSkierRanking(raceId);
            return 1 + ranking
                       ?.TakeWhile(raceRanking => (raceRanking?.TimeToLeader ?? 0) < diff.Value.TotalMilliseconds)
                       .Count();
        }

        public async Task<IEnumerable<TimeDifference>?> GetSplitTimesForCurrentSkier(int raceId)
        {
            var current = await GetCurrentSkier(raceId);
            if (current == null) return null;
            return await _statService.GetTimeDataForSkierWithDifference(current.SkierId, raceId);
        }
    }
}