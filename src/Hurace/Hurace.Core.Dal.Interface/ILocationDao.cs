using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface ILocationDao : IDefaultDeleteBaseDao<Location>
    {
        Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId);
        Task<bool> AddPossibleDisciplineForLocation(int locationId, int disciplineId);
        Task<bool> RemovePossibleDisciplineForLocation(int locationId, int disciplineId);
    }
}