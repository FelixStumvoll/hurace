using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Interface
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocations();
        Task<IEnumerable<Discipline>> GetDisciplinesForLocation(int locationId);
    }
}