using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface ISkierDao : IBaseDao<Skier>
    {
        Task<IEnumerable<Discipline>> GetPossibleDisciplinesForSkier(int skierId);
    }
}