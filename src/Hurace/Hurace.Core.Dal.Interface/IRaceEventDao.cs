using Hurace.Core.Dto;
using Hurace.Dal.Interface.Util;

namespace Hurace.Dal.Interface
{
    public interface IRaceEventDao : IDefaultDeleteBaseDao, IBaseDao<RaceEvent>, ISelectBaseDao<RaceEvent>
    {
        
    }
}