using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface ILocationDao : IBaseDao<Location>
    {
        Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId);
    }
}