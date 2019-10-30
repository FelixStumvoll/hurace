﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
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

        public override Task<bool> UpdateAsync(Location obj)
        {
            throw new System.NotImplementedException();
        }

        public override async Task<IEnumerable<Location>> FindAllAsync() =>
            await GeneratedQueryAsync(QueryFactory.Select<Location>().Join<Location, Country>(("countryId", "id"))
                                          .Build());

        public override Task<Location> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public LocationDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) :
            base(connectionFactory, "hurace.location", queryFactory)
        {
        }
    }
}