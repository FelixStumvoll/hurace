using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Base
{
    public interface IReadonlyDao<T> where T : class, new()
    {
        Task<IEnumerable<T>> FindAllAsync();
    }
}