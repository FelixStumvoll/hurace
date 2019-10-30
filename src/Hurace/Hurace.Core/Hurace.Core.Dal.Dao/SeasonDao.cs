using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SeasonDao : DefaultDeleteBaseDao<Season>, ISeasonDao
    {
        public SeasonDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.season", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(Season obj) =>
            await GeneratedExecutionAsync(QueryFactory
                                              .Update<Season>()
                                              .Where(("id", obj.Id))
                                              .Build(obj, "Id"));
    }
}