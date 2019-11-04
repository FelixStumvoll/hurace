using Hurace.Core.Dto;
using Hurace.Dal.Interface.Util;

namespace Hurace.Dal.Interface
{
    public interface IDisciplineDao : IBaseDao<Discipline>, ISingleIdBaseDao<Discipline>
    {
    }
}