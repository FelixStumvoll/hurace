using System.Collections.Generic;
using System.Linq;
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
                            .Join<Location, Country>((nameof(Location.CountryId), nameof(Country.Id)))
                            .Join<Race, Season>((nameof(Race.SeasonId), nameof(Season.Id)))
                            .Join<Race, Discipline>((nameof(Race.DisciplineId), nameof(Discipline.Id)))
                            .Join<Race, RaceState>((nameof(Race.RaceStateId), nameof(RaceState.Id)))
                            .Join<Race, Gender>((nameof(Race.GenderId), nameof(Gender.Id)));


        private SelectStatementBuilder<Race> ActiveRaceQuery() => DefaultSelectQuery()
            .Where<RaceState>((nameof(RaceState.Id), (int) Domain.Enums.RaceState.Running));
        public Task<IEnumerable<Race>> GetActiveRaces() =>
            GeneratedQueryAsync(ActiveRaceQuery().Build());

        public async Task<Race?> GetActiveRaceById(int raceId) => 
            (await GeneratedQueryAsync(ActiveRaceQuery().Where<Race>((nameof(Race.Id), raceId)).Build())).FirstOrDefault();

        public Task<IEnumerable<Race>> GetRacesForSeasonId(int seasonId) =>
            GeneratedQueryAsync(DefaultSelectQuery().Where<Race>((nameof(Race.SeasonId), seasonId)).Build());
    }
}