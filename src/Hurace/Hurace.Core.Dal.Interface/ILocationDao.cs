using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface ILocationDao : IDefaultCrudDao<Location>
    {
        Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId);
        Task<bool> InsertPossibleDisciplineForLocation(int locationId, int disciplineId);
        Task<bool> DeletePossibleDisciplineForLocation(int locationId, int disciplineId);
    }
}