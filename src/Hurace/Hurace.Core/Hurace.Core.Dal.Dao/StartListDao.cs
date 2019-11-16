using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Common.StatementBuilder;
using Hurace.Core.Common.StatementBuilder.ConcreteStatementBuilder;
using Hurace.Core.Dal.Dao.Base;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class StartListDao : CrudDao<StartList>, IStartListDao
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

        private async Task<IEnumerable<StartList>> GetStartListEntriesByState(int raceId,
            Constants.StartState startListState) =>
            await GeneratedQueryAsync(StatementFactory.Select<StartList>()
                                                      .Join<StartList, Skier>((nameof(StartList.SkierId), nameof(Skier.Id)))
                                                      .Join<Skier, Country>((nameof(Skier.CountryId), nameof(Country.Id)))
                                                      .Where<StartList>(
                                                          (nameof(StartList.StartStateId), (int) startListState), (nameof(StartList.RaceId), raceId))
                                                      .Build());
        
        public async Task<StartList?> GetNextSkierForRace(int raceId) =>
            (await GetStartListEntriesByState(raceId, Constants.StartState.Upcoming)).FirstOrDefault();

        private protected override SelectStatementBuilder<StartList> DefaultSelectQuery() =>
            StatementFactory.Select<StartList>()
                            .Join<StartList, Skier>((nameof(StartList.SkierId), nameof(Skier.Id)))
                            .Join<Skier, Country>((nameof(Skier.CountryId), nameof(Country.Id)))
                            .Join<Skier, Gender>((nameof(Skier.GenderId), nameof(Gender.Id)))
                            .Join<StartList, StartState>((nameof(StartList.StartStateId), nameof(StartState.Id)))
                            .Join<StartList, Race>((nameof(StartList.RaceId), nameof(Race.Id)));

        public async Task<StartList?> FindByIdAsync(int skierId, int raceId) =>
            (await GeneratedQueryAsync(DefaultSelectQuery()
                                       .Where<StartList>((nameof(StartList.SkierId), skierId), (nameof(StartList.RaceId), raceId)).Build()))
            .SingleOrDefault();

        public async Task<bool> DeleteAsync(int raceId, int skierId) =>
            await ExecuteAsync(
                $"delete from {TableName} where raceId=@ri and skierId=@si",
                ("@ri", raceId), ("@si", skierId));

        public async Task<StartList?> GetCurrentSkierForRace(int raceId) =>
            (await GetStartListEntriesByState(raceId, Constants.StartState.Running)).SingleOrDefault();

        public override async Task<bool> InsertAsync(StartList obj) => 
            await GeneratedNonQueryAsync(StatementFactory
                                          .Insert<StartList>()
                                          .WithKey()
                                          .Build(obj));

//        public override async Task<bool> UpdateAsync(StartList obj) => 
//            await GeneratedNonQueryAsync(StatementFactory.Update<StartList>().WithKey().WhereId(obj).Build(obj));
    }
}