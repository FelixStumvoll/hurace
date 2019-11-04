using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Util
{
    public interface IReadonlyBaseDao<T> where T : class, new()
    {
        Task<IEnumerable<T>> FindAllAsync();
    }
}