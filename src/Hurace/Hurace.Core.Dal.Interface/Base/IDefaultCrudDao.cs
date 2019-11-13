using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Base
{
    public interface IDefaultCrudDao<T> : ICrudDao<T>, IDefaultReadonlyDao<T> where T : class, new()
    {
        Task<int> InsertGetIdAsync(T obj);
        Task<bool> DeleteAsync(int id);
    }
}