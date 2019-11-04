using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Util
{
    public interface ICrudDao<T> : IReadonlyDao<T> where T : class, new()
    {
        Task<bool> UpdateAsync(T obj);
        Task<bool> InsertAsync(T obj);
        Task DeleteAllAsync();
    }
}