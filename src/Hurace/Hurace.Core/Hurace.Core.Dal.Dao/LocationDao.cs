using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class LocationDao : DefaultBaseDao<Location>, ILocationDao
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
                .Join<Location, Country>(("countryId", "id"));
        
        public override async Task DeleteAllAsync()
        {
            await ExecuteAsync("delete from hurace.PossibleDiscipline");
            await base.DefaultDeleteAll();
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            await ExecuteAsync("delete from hurace.PossibleDiscipline where locationId = @id", ("@id", id));
            return await base.DefaultDelete(id);
        }
    }
}