using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceStateDao : ReadonlyBaseDao<RaceState>, IRaceStateDao
    {
        public RaceStateDao(QueryFactory queryFactory, IConnectionFactory connectionFactory) : base(
            queryFactory, "hurace.RaceState", connectionFactory)
        {
        }
    }
}