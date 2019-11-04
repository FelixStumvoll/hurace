using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Util
{
    public interface ISingleIdReadonlyDao<T> : IReadonlyBaseDao<T> where T: class, new()
    {
        Task<T?> FindByIdAsync(int id);
    }
}