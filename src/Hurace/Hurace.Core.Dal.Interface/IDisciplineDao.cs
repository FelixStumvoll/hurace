using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface IDisciplineDao : IBaseDao<Discipline>, IDefaultDeleteBaseDao<Discipline>,
        ISelectBaseDao<Discipline>
    {
    }
}