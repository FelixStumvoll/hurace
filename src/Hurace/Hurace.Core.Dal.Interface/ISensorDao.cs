using Hurace.Core.Dto;
using Hurace.Dal.Interface.Util;

namespace Hurace.Dal.Interface
{
    public interface ISensorDao : IBaseDao<Sensor>,ISingleIdBaseDao<Sensor>
    {
        
    }
}