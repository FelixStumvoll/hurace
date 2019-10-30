using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SensorDao : BaseDao<Sensor>, ISensorDao
    {
        public SensorDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.sensor", queryFactory)
        {
        }

        public override Task<bool> UpdateAsync(Sensor obj)
        {
            throw new System.NotImplementedException();
        }
    }
}