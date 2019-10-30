using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class CountryDao : DefaultDeleteBaseDao<Country>, ICountryDao
    {
        public CountryDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.country", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(Country obj) =>
            await GeneratedExecutionAsync(QueryFactory
                                              .Update<Country>()
                                              .Where(("id", obj.Id))
                                              .Build(obj, "Id"));
    }
}