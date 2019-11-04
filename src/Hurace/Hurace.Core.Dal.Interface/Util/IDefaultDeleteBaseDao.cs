using System.Threading.Tasks;

namespace Hurace.Dal.Interface.Util
{
    public interface IDefaultDeleteBaseDao
    {
        Task<bool> DeleteAsync(int id);
    }
}