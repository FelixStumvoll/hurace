using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class DisciplineDao : BaseDao<Discipline>, IDisciplineDao
    {
        public DisciplineDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.discipline", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(Discipline obj) => 
            await GeneratedExecutionAsync(QueryFactory.Update<Discipline>().Where(("id", obj.Id)).Build(obj));
    }
}