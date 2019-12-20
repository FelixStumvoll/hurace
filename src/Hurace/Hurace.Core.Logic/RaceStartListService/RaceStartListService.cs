using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Logic.RaceStartListService
{
    public class RaceStartListService : IRaceStartListService
    {
        private readonly IStartListDao _startListDao;
        private readonly ISkierDao _skierDao;

        public RaceStartListService(IStartListDao startListDao, ISkierDao skierDao)
        {
            _startListDao = startListDao;
            _skierDao = skierDao;
        }

        public async Task<bool> UpdateStartList(Race race, IEnumerable<StartList> startList)
        {
            await _startListDao.DeleteAllForRace(race.Id);
            foreach (var sl in startList) await _startListDao.InsertAsync(sl);

            return true;
        }

        public Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId) =>
            _skierDao.FindAvailableSkiersForRace(raceId);

        public Task<IEnumerable<StartList>> GetStartListForRace(int raceId) =>
            _startListDao.GetStartListForRace(raceId);

        public async Task<bool?> IsStartListDefined(int raceId) =>
            (await _startListDao.CountStartListForRace(raceId) ?? 0) != 0;
        
        public Task<StartList?> GetStartListById(int skierId, int raceId) =>
            _startListDao.FindByIdAsync(skierId, raceId);
    }
}