using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Dal.Interface
{
    public interface IBaseDao<T> : IReadonlyBaseDao<T> where T : class
    {
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(T obj);
        Task<bool> InsertAsync(T obj);
        Task DeleteAllAsync();
    }
}