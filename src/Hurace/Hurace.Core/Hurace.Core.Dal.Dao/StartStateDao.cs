using Hurace.Core.Common;
using Hurace.Core.Common.QueryBuilder;
using Hurace.Core.Dal.Dao.Base;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class StartStateDao : DefaultReadonlyDao<StartState>, IStartStateDao
    {
        public StartStateDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
           connectionFactory , "hurace.StartState", statementFactory)
        {
        }
    }
}