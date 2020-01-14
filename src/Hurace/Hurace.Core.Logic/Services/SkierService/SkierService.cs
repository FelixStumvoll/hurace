using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public SkierService(ISkierDao skierDao)
        {
            _skierDao = skierDao;
        }

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Skier>> GetAllSkiers() => _skierDao.FindAllAsync();

        [ExcludeFromCodeCoverage]
        public Task<Skier?> GetSkierById(int id) => _skierDao.FindByIdAsync(id);
        
        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Discipline>> GetDisciplinesForSkier(int id) => 
            _skierDao.GetPossibleDisciplinesForSkier(id);

        [ExcludeFromCodeCoverage]
        public Task<bool> UpdateSkier(Skier skier) => _skierDao.UpdateAsync(skier);

        [ExcludeFromCodeCoverage]
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
    }
}