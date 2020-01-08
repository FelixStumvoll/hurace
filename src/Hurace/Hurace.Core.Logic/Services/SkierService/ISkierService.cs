using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Logic.Services.SkierService
{
    public interface ISkierService
    {
        Task<IEnumerable<Skier>> GetAllSkiers();
        Task<Skier?> GetSkierById(int id);
        Task<IEnumerable<Discipline>> GetDisciplinesForSkier(int id);
        Task<bool> UpdateSkier(Skier skier);
        Task<int?> CreateSkier(Skier skier);
        Task<bool> UpdatePossibleDisciplines(int skierId, IEnumerable<int> disciplines);
    }
}