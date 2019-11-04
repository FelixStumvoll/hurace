using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Util
{
    public interface IDefaultReadonlyDao<T> : IReadonlyDao<T> where T : class, new()
    {
        Task<T?> FindByIdAsync(int id);
    }
}