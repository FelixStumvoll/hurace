using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Util;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Logic.Services.SkierService
{
    public class SkierService : ISkierService
    {
        private readonly ISkierDao _skierDao;
        private readonly ITimeDataDao _timeDataDao;

        public SkierService(ISkierDao skierDao, ITimeDataDao timeDataDao)
        {
            _skierDao = skierDao;
            _timeDataDao = timeDataDao;
        }

        public Task<IEnumerable<Skier>> GetAllSkiers() => _skierDao.FindAllAsync();

        public Task<Skier?> GetSkierById(int id) => _skierDao.FindByIdAsync(id);
        public Task<IEnumerable<Discipline>> GetDisciplinesForSkier(int id) => 
            _skierDao.GetPossibleDisciplinesForSkier(id);

        public Task<bool> UpdateSkier(Skier skier) => _skierDao.UpdateAsync(skier);

        public Task<int?> CreateSkier(Skier skier) => _skierDao.InsertGetIdAsync(skier);

        public async Task<bool> UpdatePossibleDisciplines(int skierId, IEnumerable<int> disciplines)
        {
            var skier = await GetSkierById(skierId);
            if (skier == null) return false;
            using var scope = ScopeBuilder.BuildTransactionScope();
            await _skierDao.DeleteAllPossibleDisciplineForSkier(skierId);
            foreach (var discipline in disciplines)
            {
                await _skierDao.InsertPossibleDisciplineForSkier(skierId, discipline);
            }
            scope.Complete();
            return true;
        }
        
        public async Task<IEnumerable<RaceRanking>> GetResultsForSkier(int skierId)
        {
            return Enumerable.Empty<RaceRanking>();
        }
    }
}