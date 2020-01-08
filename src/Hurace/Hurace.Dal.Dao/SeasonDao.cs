using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
{
    public class SeasonDao : DefaultCrudDao<Season>, ISeasonDao
    {
        public SeasonDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.season", statementFactory)
        {
        }

        public Task<int?> CountRacesForSeason(int seasonId)
        {
            return ExecuteScalarAsync("select count(*) from hurace.Race where seasonId like @sid", ("@sid", seasonId));
        }
    }
}