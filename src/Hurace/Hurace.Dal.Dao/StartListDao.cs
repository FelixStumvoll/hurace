using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.Mapper;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
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
                                                s.imageUrl,
                                                s.retired,
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
            Domain.Enums.StartState startListState) =>
            await GeneratedQueryAsync(DefaultSelectQuery()
                                      .Where<StartList>(
                                          (nameof(StartList.StartStateId), (int) startListState),
                                          (nameof(StartList.RaceId), raceId))
                                      .Build());

        public async Task<StartList?> GetNextSkierForRace(int raceId)
        {
            var builtStatementData = DefaultSelectQuery()
                                     .Where<StartList>((nameof(StartList.StartStateId),
                                                           (int) Domain.Enums.StartState.Upcoming),
                                                       (nameof(StartList.RaceId), raceId)).Build();
            builtStatementData.statement = builtStatementData.statement.Replace("select", "select top 1");
            builtStatementData.statement = builtStatementData.statement +=
                $" order by {TableName}.{nameof(StartList.StartNumber)} asc";
            return (await GeneratedQueryAsync(builtStatementData)).FirstOrDefault();
        }

        public Task<IEnumerable<StartList>> GetRemainingStartListForRace(int raceId) =>
            GeneratedQueryAsync(DefaultSelectQuery()
                                                .Where<StartList>((nameof(StartList.StartStateId),
                                                                      Domain.Enums.StartState.Upcoming)).Build());

        private protected override SelectStatementBuilder<StartList> DefaultSelectQuery() =>
            StatementFactory.Select<StartList>()
                            .Join<StartList, Skier>((nameof(StartList.SkierId), nameof(Skier.Id)))
                            .Join<Skier, Country>((nameof(Skier.CountryId), nameof(Country.Id)))
                            .Join<Skier, Gender>((nameof(Skier.GenderId), nameof(Gender.Id)))
                            .Join<StartList, StartState>((nameof(StartList.StartStateId), nameof(StartState.Id)))
                            .Join<StartList, Race>((nameof(StartList.RaceId), nameof(Race.Id)));

        public async Task<StartList?> FindByIdAsync(int skierId, int raceId) =>
            (await GeneratedQueryAsync(DefaultSelectQuery()
                                       .Where<StartList>((nameof(StartList.SkierId), skierId),
                                                         (nameof(StartList.RaceId), raceId)).Build()))
            .SingleOrDefault();

        public async Task<bool> DeleteAsync(int raceId, int skierId) =>
            await ExecuteAsync(
                $"delete from {TableName} where raceId=@ri and skierId=@si",
                ("@ri", raceId), ("@si", skierId));

        public Task<bool> DeleteAllForRace(int raceId) =>
            ExecuteAsync($"delete from {TableName} where raceId=@rid", ("@rid", raceId));

        public async Task<IEnumerable<StartList>> GetDisqualifiedSkierForRace(int raceId) =>
            await QueryAsync<StartList>(
                "select * from hurace.StartListQuery where raceId = @rid and (startStateId = @ssi or startStateId = @ssii)",
                new MapperConfig()
                    .AddMapping<Skier>((nameof(StartList.SkierId), nameof(Skier.Id)))
                    .AddMapping<Country>((nameof(Skier.CountryId), nameof(Country.Id)))
                    .AddMapping<Gender>((nameof(Skier.GenderId), nameof(Gender.Id)))
                    .AddMapping<StartState>((nameof(StartList.StartStateId), nameof(StartState.Id))),
                ("@rid", raceId), ("@ssi", (int) Domain.Enums.StartState.Canceled),
                ("@ssii", (int) Domain.Enums.StartState.Disqualified));

        public Task<int?> CountStartListForRace(int raceId) =>
            ExecuteScalarAsync($"select count(*) from {TableName} where raceId=@rid", ("@rid", raceId));

        public async Task<StartList?> GetCurrentSkierForRace(int raceId) =>
            (await GetStartListEntriesByState(raceId, Domain.Enums.StartState.Running)).SingleOrDefault();

        public override async Task<bool> InsertAsync(StartList obj) =>
            await GeneratedNonQueryAsync(StatementFactory
                                         .Insert<StartList>()
                                         .WithKey()
                                         .Build(obj));

//        public override async Task<bool> UpdateAsync(StartList obj) => 
//            await GeneratedNonQueryAsync(StatementFactory.Update<StartList>().WithKey().WhereId(obj).Build(obj));
    }
}