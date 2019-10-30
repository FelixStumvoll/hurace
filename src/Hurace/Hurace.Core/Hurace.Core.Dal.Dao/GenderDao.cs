using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class GenderDao : ReadonlyBaseDao<Gender>, IGenderDao
    {
        public GenderDao(QueryFactory queryFactory, IConnectionFactory connectionFactory) : base(
            queryFactory, "hurace.Gender", connectionFactory)
        {
        }
    }
}