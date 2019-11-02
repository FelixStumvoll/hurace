using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class StartStateDao : BaseDao<StartState>, IStartStateDao
    {
        public StartStateDao(StatementFactory statementFactory, IConnectionFactory connectionFactory) : base(
           connectionFactory , "hurace.StartState", statementFactory)
        {
        }
    }
}