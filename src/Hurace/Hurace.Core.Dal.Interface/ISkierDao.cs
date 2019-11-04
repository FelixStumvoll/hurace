using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using Hurace.Dal.Interface.Util;

namespace Hurace.Dal.Interface
{
    public interface ISkierDao : IDefaultCrudDao<Skier>
    {
        Task<IEnumerable<Discipline>> GetPossibleDisciplinesForSkier(int skierId);
        Task<bool> InsertPossibleDisciplineForSkier(int skierId, int disciplineId);
        Task<bool> DeletePossibleDisciplineForSkier(int skierId, int disciplineId);
    }
}