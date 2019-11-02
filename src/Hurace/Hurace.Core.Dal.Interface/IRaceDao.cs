using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface IRaceDao : IBaseDao<Race>,IDefaultDeleteBaseDao<Race>, ISelectBaseDao<Race>
    {
    }
}