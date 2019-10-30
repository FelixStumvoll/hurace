using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Dal.Interface
{
    public interface IReadonlyBaseDao<T> where T : class
    {
        Task<IEnumerable<T>> FindAllAsync();
        Task<T?> FindByIdAsync(int id);
    }
}