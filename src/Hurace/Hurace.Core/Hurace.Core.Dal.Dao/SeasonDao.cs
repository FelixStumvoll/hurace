using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SeasonDao : DefaultDeleteBaseDao<Season>, ISeasonDao
    {
        public SeasonDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.season", statementFactory)
        {
        }
    }
}