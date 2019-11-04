using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using Hurace.Dal.Interface.Util;

namespace Hurace.Dal.Interface
{
    public interface ILocationDao : IDefaultDeleteBaseDao, IBaseDao<Location>, ISelectBaseDao<Location>
    {
        Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId);
        Task<bool> InsertPossibleDisciplineForLocation(int locationId, int disciplineId);
        Task<bool> DeletePossibleDisciplineForLocation(int locationId, int disciplineId);
    }
}