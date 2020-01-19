using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Service
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineDao _disciplineDao;

        public DisciplineService(IDisciplineDao disciplineDao)
        {
            _disciplineDao = disciplineDao;
        }

        public Task<IEnumerable<Discipline>> GetAllDisciplines() => _disciplineDao.FindAllAsync();
    }
}