using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface IRaceDataDao : IBaseDao<RaceData>, IDefaultDeleteBaseDao<RaceData>, ISelectBaseDao<RaceData>
    {
        
    }
}