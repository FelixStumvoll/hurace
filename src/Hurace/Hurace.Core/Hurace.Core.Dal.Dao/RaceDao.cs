using System.Collections.Generic;
using System.Linq;
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
        public RaceDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) :
            base(connectionFactory, "hurace.race", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(Race obj) =>
            await GeneratedExecutionAsync(QueryFactory.Update<Race>()
                                              .Where(("id", obj.Id))
                                              .Build(obj, "Id"));


        private SelectQueryBuilder<Race> RaceDefaultQuery() =>
            QueryFactory.Select<Race>()
                .Join<Race, Location>(("locationId", "id"))
                .Join<Race, Season>(("seasonId", "id"))
                .Join<Race, Discipline>(("disciplineId", "id"))
                .Join<Race, RaceState>(("raceStateId", "id"));

        public override async Task<IEnumerable<Race>> FindAllAsync() =>
            await GeneratedQueryAsync(RaceDefaultQuery().Build());

        public override async Task<Race> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(RaceDefaultQuery()
                                           .Where<Race>(("id", id))
                                           .Build()))
            .SingleOrDefault();
        
        

       
    }
}