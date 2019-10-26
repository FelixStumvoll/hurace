using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class LocationDao : BaseDao<Location>, ILocationDao
    {
        public LocationDao(IConnectionFactory connectionFactory) : base(
            connectionFactory, "hurace.location")
        {
        }

        public Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Location obj)
        {
            throw new System.NotImplementedException();
        }
    }
}