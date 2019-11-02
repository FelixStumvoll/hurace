using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface ISensorDao : IBaseDao<Sensor>,IDefaultDeleteBaseDao<Sensor>, ISelectBaseDao<Sensor>
    {
        
    }
}