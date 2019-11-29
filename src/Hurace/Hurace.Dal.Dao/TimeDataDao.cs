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
            var topSection = count <= 0 ? "" : $" top {count}";
            return QueryAsync<RaceRanking>(
                $@"select{topSection} * from hurace.TimeDataRanking where raceId=@raceId
                                                order by raceTime asc",
                new MapperConfig()
                    .AddMapping<Country>(("countryId", nameof(Country.Id)))
                    .Include<StartList>()
                    .AddMapping<Skier>(("skierId",nameof(Skier.Id)))
                    .AddMapping<Gender>(("genderId", nameof(Gender.Id))),
                ("@raceId", raceId));
        }

        public async Task<bool> DeleteAsync(int skierId, int raceId, int sensorId) =>
            await ExecuteAsync($"delete from {TableName} where skierId=@sId and raceId=@rId and sensorId=@sensorId",
                               ("@sid", skierId), ("@rId", raceId), ("@sensorId", sensorId));

        private protected override SelectStatementBuilder<TimeData> DefaultSelectQuery() =>
            StatementFactory
                .Select<TimeData>()
                .Join<TimeData, StartList>((nameof(TimeData.RaceId), nameof(StartList.RaceId)),
                                             (nameof(TimeData.SkierId), nameof(StartList.SkierId)))
                .Join<TimeData, SkierEvent>((nameof(TimeData.SkierEventId), nameof(SkierEvent.Id)))
                .Join<SkierEvent, RaceData>((nameof(SkierEvent.RaceDataId), nameof(RaceData.Id)))
                .Join<TimeData, Sensor>((nameof(TimeData.SensorId), nameof(Sensor.Id)))
                .Join<StartList, Skier>((nameof(StartList.SkierId), nameof(Skier.Id)))
                .Join<Skier, Country>((nameof(Skier.CountryId), nameof(Country.Id)));

        public async Task<TimeData?> FindByIdAsync(int skierId, int raceId, int sensorId) =>
            (await GeneratedQueryAsync(DefaultSelectQuery()
                                       .Where<TimeData>((nameof(TimeData.SkierId), skierId),
                                                        (nameof(TimeData.RaceId), raceId),
                                                        (nameof(TimeData.SensorId), sensorId))
                                       .Build()))
            .SingleOrDefault();
    }
}