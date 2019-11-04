using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceDao : BaseDao<Race>, IRaceDao
    {
        public RaceDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) :
            base(connectionFactory, "hurace.race", statementFactory)
        {
        }

        private protected override SelectStatementBuilder<Race> DefaultSelectQuery() =>
            StatementFactory.Select<Race>()
                            .Join<Race, Location>(("locationId", "id"))
                            .Join<Race, Season>(("seasonId", "id"))
                            .Join<Race, Discipline>(("disciplineId", "id"))
                            .Join<Race, RaceState>(("raceStateId", "id"));
    }
}