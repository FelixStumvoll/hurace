using System.Threading.Tasks;

namespace Hurace.Dal.Interface
{
    public interface IDefaultDeleteBaseDao<T> : IBaseDao<T> where T : class
    {
        Task<bool> DeleteAsync(int id);
    }
}