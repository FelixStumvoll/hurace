using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class GenderDao : BaseDao<Gender>, IGenderDao
    {
        public GenderDao(StatementFactory statementFactory, IConnectionFactory connectionFactory) : base(
            connectionFactory,"hurace.Gender", statementFactory)
        {
        }
    }
}