using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;

namespace Hurace.Dal.Interface
{
    public interface IBaseDao<T> where T : class
    {
        Task<IEnumerable<T>> FindAllAsync();
        Task<T?> FindByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(T obj);
        Task<bool> InsertAsync(T obj);
    }
}