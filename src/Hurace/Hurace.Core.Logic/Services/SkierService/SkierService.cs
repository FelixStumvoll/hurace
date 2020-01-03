using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
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

        public async Task<IEnumerable<RaceRanking>> GetResultsForSkier(int skierId)
        {
            return Enumerable.Empty<RaceRanking>();
        }
    }
}