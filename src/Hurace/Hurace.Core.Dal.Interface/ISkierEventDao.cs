using Hurace.Core.Dto;
using Hurace.Dal.Interface.Util;

namespace Hurace.Dal.Interface
{
    public interface ISkierEventDao : IBaseDao<SkierEvent>, ISelectBaseDao<SkierEvent>,IDefaultDeleteBaseDao
    {
        
    }
}