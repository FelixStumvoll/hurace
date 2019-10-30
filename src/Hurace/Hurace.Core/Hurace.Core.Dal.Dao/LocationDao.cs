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
    public class LocationDao : BaseDao<Location>, ILocationDao
    {
        //todo load in country
        public async Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId) =>
            await QueryAsync<Discipline>("select * from hurace.PossibleDiscipline where locationId = @id",
                                         queryParams: ("@id", locationId));

        public override async Task<bool> UpdateAsync(Location obj) =>
            await GeneratedExecutionAsync(QueryFactory.Update<Location>()
                                              .Where(("id", obj.Id))
                                              .Build(obj, "Id"));

        private SelectQueryBuilder<Location> DefaultLocationQuery() =>
            QueryFactory.Select<Location>().Join<Location, Country>(("countryId", "id"));

        public override async Task<IEnumerable<Location>> FindAllAsync() =>
            await GeneratedQueryAsync(DefaultLocationQuery()
                                          .Build());

        public override async Task<Location> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(DefaultLocationQuery()
                                           .Where<Location>(("id", id))
                                           .Build()))
            .SingleOrDefault();

        public LocationDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) :
            base(connectionFactory, "hurace.location", queryFactory)
        {
        }
    }
}