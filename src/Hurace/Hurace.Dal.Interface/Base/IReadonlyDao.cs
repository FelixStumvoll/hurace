using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Domain.Enums;

namespace Hurace.Dal.Interface.Base
{
    public interface IReadonlyDao<T> where T : class, new()
    {
        Task<IEnumerable<T>> FindAllAsync();
    }
}