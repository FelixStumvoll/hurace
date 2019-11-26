using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Core.Api
{
    public interface IHuraceCore
    {
        Task<ICollection<Gender>> GetGenders();
        Task<ICollection<Location>> GetLocations();
        Task<ICollection<Discipline>> GetDisciplines();
    }
}