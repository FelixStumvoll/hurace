using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class CountryDao : BaseDao<Country>, ICountryDao
    {
        public CountryDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.country", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(Country obj) =>
            await GeneratedExecutionAsync(_queryFactory.Update<Country>().Where(("Id", obj.Id)).Build(obj));
        
        public override Task<Country> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}