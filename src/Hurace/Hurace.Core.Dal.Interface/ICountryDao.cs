using Hurace.Core.Dto;
using Hurace.Dal.Interface.Util;

namespace Hurace.Dal.Interface
{
    public interface ICountryDao : IDefaultDeleteBaseDao, IBaseDao<Country>, ISelectBaseDao<Country>
    {
    }
}