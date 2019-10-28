using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
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
    }
}