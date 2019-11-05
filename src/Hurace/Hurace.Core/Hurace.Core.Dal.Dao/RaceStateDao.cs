using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceStateDao : DefaultReadonlyDao<RaceState>, IRaceStateDao
    {
        public RaceStateDao( IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.RaceState", statementFactory)
        {
        }
    }
}