using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface ISkierDao : IDefaultCrudDao<Skier>
    {
        Task<IEnumerable<Skier>> FindAvailableSkiersForRace(int raceId);
        Task<IEnumerable<Discipline>> GetPossibleDisciplinesForSkier(int skierId);
        Task<bool> InsertPossibleDisciplineForSkier(int skierId, int disciplineId);
        Task<bool> DeletePossibleDisciplineForSkier(int skierId, int disciplineId);
    }
}