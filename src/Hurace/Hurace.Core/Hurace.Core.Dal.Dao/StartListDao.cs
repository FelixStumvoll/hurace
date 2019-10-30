using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class StartListDao : BaseDao<StartList>, IStartListDao
    {
        public StartListDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.StartList", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(StartList obj) =>
            await GeneratedExecutionAsync(QueryFactory.Update<StartList>()
                                              .Where(("skierId", obj.RaceId), ("raceId", obj.RaceId))
                                              .Build(obj, "SkierId", "RaceId"));
    }
}