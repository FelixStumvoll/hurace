using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
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
                            .Join<Race, Season>((nameof(Race.SeasonId), nameof(Season.Id)))
                            .Join<Race, Discipline>((nameof(Race.DisciplineId), nameof(Discipline.Id)))
                            .Join<Race, RaceState>((nameof(Race.RaceStateId), nameof(RaceState.Id)))
                            .Join<Race, Gender>((nameof(Race.GenderId), nameof(Gender.Id)));

        public Task<IEnumerable<Race>> GetActiveRaces() =>
            GeneratedQueryAsync(DefaultSelectQuery()
                                .Where<RaceState>((nameof(RaceState.Id), (int) Constants.RaceState.Running)).Build());

        public Task<IEnumerable<Race>> GetRaceForSeasonId(int seasonId) =>
            GeneratedQueryAsync(DefaultSelectQuery().Where<Race>((nameof(Race.SeasonId), seasonId)).Build());
    }
}