using Hurace.Core.Common;
using Hurace.Core.Common.StatementBuilder;
using Hurace.Core.Common.StatementBuilder.ConcreteStatementBuilder;
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
                            .Join<Race, Location>((nameof(Race.LocationId), nameof(Location.Id)))
                            .Join<Race, Season>((nameof(Race.SeasonId),nameof(Season.Id)))
                            .Join<Race, Discipline>((nameof(Race.DisciplineId), nameof(Discipline.Id)))
                            .Join<Race, RaceState>((nameof(Race.RaceStateId), nameof(RaceState.Id)))
                            .Join<Race, Gender>((nameof(Race.GenderId), nameof(Gender.Id)));
    }
}