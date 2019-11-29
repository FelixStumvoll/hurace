using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
{
    public class GenderDao : DefaultReadonlyDao<Gender>, IGenderDao
    {
        public GenderDao( IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory,"hurace.Gender", statementFactory)
        {
        }
    }
}