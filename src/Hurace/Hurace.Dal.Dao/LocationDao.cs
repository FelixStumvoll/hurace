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
    public class LocationDao : DefaultCrudDao<Location>, ILocationDao
    {
        public LocationDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) :
            base(connectionFactory, "hurace.location", statementFactory)
        {
        }

        public async Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId) =>
            await QueryAsync<Discipline>(@"select 
            d.id, d.disciplineName
            from hurace.PossibleDiscipline as pd
            join hurace.Discipline as d on d.id = pd.disciplineId
            where locationId = @id", queryParams: ("@id", locationId));

        public async Task<bool> InsertPossibleDisciplineForLocation(int locationId, int disciplineId) =>
            await ExecuteAsync("insert into hurace.PossibleDiscipline values(@li, @di)", 
                               ("@di", disciplineId), ("@li", locationId));

        public async Task<bool> DeletePossibleDisciplineForLocation(int locationId, int disciplineId) =>
            await ExecuteAsync(
                @"delete pd 
                           from hurace.PossibleDiscipline as pd
                           where pd.disciplineId=@di and pd.locationId=@li", 
                ("@di", disciplineId), ("@li", locationId));

        private protected override SelectStatementBuilder<Location> DefaultSelectQuery() =>
            StatementFactory
                .Select<Location>()
                .Join<Location, Country>((nameof(Location.CountryId), nameof(Country.Id)));
        
        public override async Task DeleteAllAsync()
        {
            await ExecuteAsync("delete from hurace.PossibleDiscipline");
            await base.DeleteAllAsync();
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            await ExecuteAsync("delete from hurace.PossibleDiscipline where locationId = @id", ("@id", id));
            return await base.DeleteAsync(id);
        }
    }
}