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
    public class StartListDao : BaseDao<StartList>, IStartListDao
    {
        public StartListDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.StartList", statementFactory)
        {
        }
        
        public async Task<IEnumerable<StartList>> GetStartListForRace(int raceId) =>
            await QueryAsync<StartList>(@"select
                                                sl.raceId,
                                                s.id as skierId,
                                                s.firstName,
                                                s.lastName,
                                                s.dateOfBirth,
                                                s.genderId,
                                                s.countryId,
                                                c.countryName,
                                                c.countryCode,
                                                sl.startNumber,
                                                sl.startStateId,
                                                ss.startStateDescription
                                                from hurace.StartList as sl
                                                join hurace.skier as s on s.id = sl.skierId
                                                join hurace.country as c on c.id = s.countryId
                                                join hurace.Gender as g on g.id = s.genderId
                                                join hurace.StartState as ss on ss.id = sl.startStateId
                                                where sl.raceId = @id
                                                order by sl.startNumber asc",
                                        new MapperConfig()
                                            .Include<Skier>()
                                            .Include<Country>()
                                            .AddMapping<Country>(("countryId", "Id"))
                                            .AddMapping<Skier>(("skierId", "Id")),
                                        ("@id", raceId));

        private async Task<IEnumerable<StartList>> GetStartListEntriesByState(int raceId, int startListState) =>
            (await QueryAsync<StartList>(@"select s.id,
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
                                             .Include<Skier>()
                                             .Include<Country>(),
                                         ("@id", raceId), ("@sls", startListState)));

        public async Task<StartList?> GetNextSkierForRace(int raceId) =>
            (await GetStartListEntriesByState(raceId, 1)).FirstOrDefault();

        public async Task<bool> DeleteAsync(int raceId, int skierId) =>
            await ExecuteAsync(
                $"delete from {TableName} where raceId=@ri, skierId=@si",
                ("@ri", raceId), ("@si", skierId));

        public async Task<StartList?> GetCurrentSkierForRace(int raceId) =>
            (await GetStartListEntriesByState(raceId, 2)).SingleOrDefault();

        public override async Task<bool> InsertAsync(StartList obj) => 
            await GeneratedExecutionAsync(StatementFactory.Insert<StartList>().WithKey().Build(obj));

        public override async Task<int> InsertGetIdAsync(StartList obj) => 
            await GeneratedExecutionWithIdAsync(StatementFactory.Insert<StartList>().WithKey().Build(obj));
    }
}