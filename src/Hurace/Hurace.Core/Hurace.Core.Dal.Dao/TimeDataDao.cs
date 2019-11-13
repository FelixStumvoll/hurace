using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Common.QueryBuilder;
using Hurace.Core.Common.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dal.Dao.Base;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class TimeDataDao : CrudDao<TimeData>, ITimeDataDao
    {
        public TimeDataDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.TimeData", statementFactory)
        {
        }

        public override async Task<bool> InsertAsync(TimeData obj) =>
            await GeneratedNonQueryAsync(StatementFactory.Insert<TimeData>().WithKey().Build(obj));

        public Task<IEnumerable<RaceRanking>> GetRankingForRace(int raceId, int count = 0)
        {
            var topSection = count > 0 ? "" : $" top {count}";
            return QueryAsync<RaceRanking>(
                $@"select{topSection} * from hurace.TimeDataRanking where raceId=@raceId
                                                order by raceTime asc",
                new MapperConfig()
                    .AddMapping<RaceRanking>(("skierId", "SkierId"), ("raceTime", "Time"))
                    .AddMapping<Country>(("countryId", "Id"))
                    .AddMapping<StartList>(("skierId", "SkierId"))
                    .AddMapping<Skier>(("skierId", "Id"))
                    .AddMapping<Gender>(("genderId", "Id")),
                ("@raceId", raceId));
        }

        public async Task<bool> DeleteAsync(int skierId, int raceId, int sensorId) =>
            await ExecuteAsync($"delete from {TableName} where skierId=@sId and raceId=@rId and sensorId=@sensorId",
                               ("@sid", skierId), ("@rId", raceId), ("@sensorId", sensorId));

        private protected override SelectStatementBuilder<TimeData> DefaultSelectQuery() =>
            StatementFactory
                .Select<TimeData>()
                .Join<TimeData, StartList>(("skierId", "skierId"), ("raceId", "raceId"))
                .Join<TimeData, SkierEvent>(("skierEventId", "id"))
                .Join<SkierEvent, RaceData>(("raceDataId", "id"))
                .Join<TimeData, Sensor>(("sensorId", "id"))
                .Join<StartList, Skier>(("skierId", "id"))
                .Join<Skier, Country>(("countryId", "id"));

        public async Task<TimeData?> FindByIdAsync(int skierId, int raceId, int sensorId) =>
            (await GeneratedQueryAsync(DefaultSelectQuery()
                                       .Where<TimeData>(("skierId", skierId),
                                                        ("raceId", raceId),
                                                        ("sensorId", sensorId))
                                       .Build()))
            .SingleOrDefault();

//        public override async Task<bool> UpdateAsync(TimeData obj) => 
//            await GeneratedNonQueryAsync(StatementFactory.Update<TimeData>().WithKey().WhereId(obj).Build(obj));
    }
}