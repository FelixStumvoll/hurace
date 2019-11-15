using System.Threading.Tasks;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Dal.Interface.Base
{
    public interface IDefaultReadonlyDao<T> : IReadonlyDao<T> where T : class, ISinglePkEntity, new()
    {
        Task<T?> FindByIdAsync(int id);
    }
}