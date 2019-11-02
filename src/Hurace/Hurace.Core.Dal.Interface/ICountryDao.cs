using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface ICountryDao : IDefaultDeleteBaseDao<Country>, IBaseDao<Country>, ISelectBaseDao<Country>
    {
    }
}