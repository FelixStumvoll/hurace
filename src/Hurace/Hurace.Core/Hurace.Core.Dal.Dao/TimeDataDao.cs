using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class TimeDataDao : BaseDao<TimeData>, ITimeDataDao
    {
        public TimeDataDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.timedata", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(TimeData obj) =>
            await GeneratedExecutionAsync(QueryFactory.Update<TimeData>()
                                              .Where(("skierId", obj.SkierId), ("raceId", obj.RaceId))
                                              .Build(obj, "SkierId", "RaceId"));
    }
}