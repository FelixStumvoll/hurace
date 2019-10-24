using AutoMapper;
using Hurace.Core.Common;
using Hurace.Core.Dto;

namespace Hurace.Core.Dal.Dao
{
    public class CountryDao : BaseDao<Country>
    {
        public CountryDao(IConnectionFactory connectionFactory, IMapper mapper) : base(
            connectionFactory, mapper, "hurace.country")
        {
        }
    }
}