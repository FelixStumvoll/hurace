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

        //todo load in country
        public async Task<IEnumerable<Discipline>> GetPossibleDisciplinesForLocation(int locationId) =>
            await QueryAsync<Discipline>("select * from hurace.PossibleDiscipline where locationId = @id",
                                         queryParams: ("@id", locationId));

        public override async Task<IEnumerable<Location>> FindAllAsync() =>
            await QueryAsync<Location>(@"select l.id,
                                                       l.countryId,
                                                       l.locationName,
                                                       c.countryCode,
                                                       c.countryName 
                                                       from hurace.Location as l
                                                       join hurace.Country as c on c.id = l.countryId",
                                       new MapperConfig().AddMapping<Country>(
                                           ("countryId", "Id"), ("countryName", "Name")));
    }
}