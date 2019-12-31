using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task<IEnumerable<Skier>> GetAllSkiers() => _skierDao.FindAllAsync();

        public Task<Skier?> GetSkierById(int id) => _skierDao.FindByIdAsync(id);
    }
}