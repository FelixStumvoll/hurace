using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
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

//        public override async Task<bool> UpdateAsync(Location obj) =>
//            (await ExecuteAsync($"update {TableName} set " +
//                                "name=@n," +
//                                "countryId=@ci," +
//                                "where id=@id", 
//                                ("@id", obj.Id),
//                                ("@n", obj.Name),
//                                ("@ci", obj.CountryId))) == 1;

        
    }
}