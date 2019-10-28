using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
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
            await GeneratedExecutionAsync(_queryFactory.Update<Race>()
                            .Where(("Id", obj.Id))
                            .Build(obj));
        
        public override Task<IEnumerable<Race>> FindAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public override Task<Race> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<TimeData>> GetRanking(int raceId)
        {
            return QueryAsync<TimeData>(@"select	s.id,
                                                s.lastName,
                                                s.dateOfBirth,
                                                s.countryId,
                                                s.genderId,
                                                s.firstName,
                                                c.countryCode,
                                                c.name as countryName,
                                                max(td.time) as raceTime
                                                from hurace.TimeData as td
                                                    join hurace.Skier as s on td.skierId = s.id
                                                join hurace.Country as c on s.countryId = c.id
                                                join hurace.StartList as sl on sl.raceId = @id and sl.skierId = s.id
                                                where td.raceId = @id and sl.startStateId = 3
                                                group by
                                                s.id,
                                                s.firstName,
                                                s.lastName,
                                                s.countryId,
                                                s.genderId,
                                                s.dateOfBirth,
                                                c.countryCode,
                                                c.name,
                                                order by raceTime desc",
                                        new MapperConfig()
                                            .AddMapping<Country>(("countryId", "Id"))
                                            .AddExclusion<Gender>()
                                            .AddExclusion<Race>(),
                                        ("@id", raceId));
        }

        public async Task<IEnumerable<StartList>> GetStartList(int raceId) =>
            await QueryAsync<StartList>(@"select
                                                sl.raceId,
                                                s.id as skierId,
                                                s.firstName,
                                                s.lastName,
                                                s.dateOfBirth,
                                                s.countryId,
                                                c.name,
                                                c.countryCode,
                                                sl.startNumber,
                                                sl.startStateId,
                                                ss.description as startStateDescription
                                                from hurace.StartList as sl
                                                join hurace.skier as s on s.id = sl.skierId
                                                join hurace.country as c on c.id = s.countryId
                                                join hurace.Gender as g on g.id = s.genderId
                                                join hurace.StartState as ss on ss.id = sl.startStateId
                                                where sl.raceId = @id
                                                order by sl.startNumber asc",
                                        new MapperConfig()
                                            .AddMapping<Country>(("countryId", "Id"))
                                            .AddMapping<Skier>(("skierId", "Id"))
                                            .AddExclusion<Gender>()
                                            .AddExclusion<Race>(),
                                        ("@id", raceId));

        public async Task<StartList?> GetCurrentSkier(int raceId) =>
            (await GetStartListEntriesByState(raceId, 2)).SingleOrDefault();

        private async Task<IEnumerable<StartList>> GetStartListEntriesByState(int raceId, int startListState)
        {
            return (await QueryAsync<StartList>(@"select s.id,
                                                s.firstName,
                                                s.lastName,
                                                s.dateOfBirth,
                                                s.countryId,
                                                c.name,
                                                c.countryCode, 
                                                sl.startNumber,
                                                sl.startStateId
                                                from hurace.StartList as sl
                                                join hurace.skier as s on s.id = sl.skierId 
                                                join hurace.country as c on c.id = s.countryId
                                                where sl.startStateId = @sls and sl.raceId = @id",
                                                new MapperConfig()
                                                    .AddExclusion<Gender>()
                                                    .AddExclusion<Race>()
                                                    .AddExclusion<StartState>(),
                                                ("@id", raceId), ("@sls", startListState)));
        }

        public async Task<StartList?> GetNextSkier(int raceId) =>
            (await GetStartListEntriesByState(raceId, 1)).FirstOrDefault();
    }
}