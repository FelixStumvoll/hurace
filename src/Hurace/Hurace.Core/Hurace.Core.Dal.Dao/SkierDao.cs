using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SkierDao : BaseDao<Skier>, ISkierDao
    {
        public SkierDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.skier", queryFactory)
        {
        }

        public override Task<bool> UpdateAsync(Skier obj)
        {
            throw new System.NotImplementedException();
        }


        public override async Task<IEnumerable<Skier>> FindAllAsync() =>
            await GeneratedQueryAsync(QueryFactory
                                          .Select<Skier>()
                                          .Join<Skier, Country>(("countryId", "id"))
                                          .Join<Skier, Gender>(("genderId", "id"))
                                          .Build());



        public override Task<Skier> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}