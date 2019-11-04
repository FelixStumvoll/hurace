using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Util
{
    public interface ISingleIdBaseDao<T> : ISingleIdReadonlyDao<T> where T : class, new()
    {
        Task<int> InsertGetIdAsync(T obj);
        Task<bool> DeleteAsync(int id);
    }
}