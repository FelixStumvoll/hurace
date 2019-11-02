using System.Threading.Tasks;

namespace Hurace.Dal.Interface
{
    public interface IDefaultDeleteBaseDao<in T> where T : class
    {
        Task<bool> DeleteAsync(int id);
    }
}