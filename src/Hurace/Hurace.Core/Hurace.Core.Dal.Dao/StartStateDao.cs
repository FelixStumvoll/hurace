using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class StartStateDao : ReadonlyBaseDao<StartState>, IStartStateDao
    {
        public StartStateDao(StatementFactory statementFactory, string tableName, IConnectionFactory connectionFactory) : base(
            statementFactory, tableName, connectionFactory)
        {
        }
    }
}