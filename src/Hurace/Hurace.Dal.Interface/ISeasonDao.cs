using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Interface
{
    public interface ISeasonDao : IDefaultCrudDao<Season>
    {
        Task<int?> CountRacesForSeason(int seasonId);
    }
}