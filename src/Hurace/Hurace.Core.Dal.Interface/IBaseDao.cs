using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;

namespace Hurace.Dal.Interface
{
    public interface IBaseDao<T> where T : class
    {
        Task<int> ExecuteAsync(string statement, params QueryParam[] queryParams);
        Task<IEnumerable<T>> QueryAsync(string statement, params QueryParam[] queryParams);
        Task<IEnumerable<T>> FindAllAsync();
        Task<T?> FindByIdAsync(int id);
        Task<IEnumerable<T>> FindAllWhereAsync(string condition, params QueryParam[] queryParams);
        Task<bool> DeleteAsync(int id);
    }
}