using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceStateDao : BaseDao<RaceState>, IRaceStateDao
    {
        public RaceStateDao(StatementFactory statementFactory, IConnectionFactory connectionFactory) : base(
            connectionFactory, "hurace.RaceState", statementFactory)
        {
        }
    }
}