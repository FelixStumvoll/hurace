using Hurace.Core.Common;
using Hurace.Core.Common.QueryBuilder;
using Hurace.Core.Common.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dal.Dao.Base;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceDao : DefaultCrudDao<Race>, IRaceDao
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
                            .Join<Race, RaceState>(("raceStateId", "id"))
                            .Join<Race, Gender>(("genderId", "id"));
    }
}