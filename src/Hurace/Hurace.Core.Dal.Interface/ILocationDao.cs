using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface ILocationDao : IDefaultDeleteBaseDao<Location>, IBaseDao<Location>, ISelectBaseDao<Location>
    {
        Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId);
        Task<bool> InsertPossibleDisciplineForLocation(int locationId, int disciplineId);
        Task<bool> DeletePossibleDisciplineForLocation(int locationId, int disciplineId);
    }
}