using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Util
{
    public interface ISelectBaseDao<T> where T : class
    {
        Task<IEnumerable<T>> FindAllAsync();
        Task<T?> FindByIdAsync(int id);
    }
}