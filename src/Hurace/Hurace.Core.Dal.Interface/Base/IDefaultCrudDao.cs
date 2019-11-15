using System.Threading.Tasks;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Dal.Interface.Base
{
    public interface IDefaultCrudDao<T> : ICrudDao<T>, IDefaultReadonlyDao<T> where T : class, ISinglePkEntity,new()
    {
        Task<int> InsertGetIdAsync(T obj);
        Task<bool> DeleteAsync(int id);
    }
}