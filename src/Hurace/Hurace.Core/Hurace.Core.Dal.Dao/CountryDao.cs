using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class CountryDao : BaseDao<Country>, ICountryDao
    {
        public CountryDao(IConnectionFactory connectionFactory) : base(
            connectionFactory, "hurace.country")
        {
        }

        public override Task<bool> UpdateAsync(Country obj)
        {
            throw new System.NotImplementedException();
        }
    }
}