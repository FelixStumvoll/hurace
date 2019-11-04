using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;
using Hurace.Dal.Interface.Util;

namespace Hurace.Dal.Interface
{
    public interface IRaceDao : IBaseDao<Race>,IDefaultDeleteBaseDao, ISelectBaseDao<Race>
    {
    }
}