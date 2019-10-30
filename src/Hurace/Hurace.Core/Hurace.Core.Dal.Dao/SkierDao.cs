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
    public class SkierDao : DefaultDeleteBaseDao<Skier>, ISkierDao
    {
        public SkierDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.skier", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(Skier obj) =>
            await GeneratedExecutionAsync(QueryFactory
                                              .Update<Skier>()
                                              .Where(("id", obj.Id))
                                              .Build(obj, "Id"));

        public async Task<IEnumerable<Discipline>> GetPossibleDisciplinesForSkier(int skierId) =>
            await QueryAsync<Discipline>("select * from hurace.discipline where skierId = @skierId",
                                         queryParams: ("@skierId", skierId));


        private SelectQueryBuilder<Skier> DefaultSkierQuery() =>
            QueryFactory
                .Select<Skier>()
                .Join<Skier, Country>(("countryId", "id"))
                .Join<Skier, Gender>(("genderId", "id"));

        public override async Task<IEnumerable<Skier>> FindAllAsync() =>
            await GeneratedQueryAsync(DefaultSkierQuery().Build());


        public override async Task<Skier> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(DefaultSkierQuery().Where<Skier>(("id", id)).Build())).SingleOrDefault();
    }
}