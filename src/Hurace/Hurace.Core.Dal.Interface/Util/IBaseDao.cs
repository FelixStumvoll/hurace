using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Util
{
    public interface IBaseDao<T> where T : class
    {
        Task<bool> UpdateAsync(T obj);
        Task<bool> InsertAsync(T obj);
        Task<int> InsertGetIdAsync(T obj);
        Task DeleteAllAsync();
    }
}