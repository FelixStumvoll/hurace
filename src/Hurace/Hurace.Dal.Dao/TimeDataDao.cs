using System;
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
using StartState = Hurace.Dal.Domain.Enums.StartState;

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

        public async Task<IEnumerable<TimeData>> GetRankingForSensor(int raceId, int sensorNumber, int count = 0)
        {
            var topSection = count <= 0 ? "" : $" top {count}";
            return await QueryAsync<TimeData>(
                $@"select{topSection} * from hurace.SensorRanking where RaceId=@rid and sensorNumber=@sid and startStateId=@ssid order by time",
                new MapperConfig()
                    .Include<StartList>()
                    .AddMapping<Country>(("countryId", nameof(Country.Id)))
                    .AddMapping<Skier>(("skierId", nameof(Skier.Id)))
                    .AddMapping<Gender>(("genderId", nameof(Gender.Id))), ("@rid", raceId),
                ("@sid", sensorNumber), ("@ssid", (int) StartState.Finished));
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

        public Task<IEnumerable<TimeData>> GetTimeDataForStartList(int skierId, int raceId) =>
            GeneratedQueryAsync(DefaultSelectQuery()
                                .Where<StartList>((nameof(StartList.RaceId), raceId),
                                                  (nameof(StartList.SkierId), skierId)).Build());

        public Task<int?> CountTimeDataForRace(int raceId) =>
            ExecuteScalarAsync($"select count(*) from {TableName} where raceId=@rid", ("@rid", raceId));

        public async Task<int?> GetAverageTimeForSensor(int raceId, int sensorNumber) =>
            await ExecuteScalarAsync(
                @$"select AVG(td.time) from {TableName} as td 
                            inner join hurace.Sensor as s on s.id = td.sensorId
                            where td.raceId=@rid and s.sensorNumber = @sn",
                ("@rid", raceId), ("@sn", sensorNumber));

        public async Task<DateTime?> GetStartTimeForStartList(int skierId, int raceId) =>
            (await QueryAsync<RaceData>(@"select 
            rd.id,
            rd.eventDateTime,
            rd.eventTypeId,
            rd.raceId
            from hurace.RaceData as rd
            join hurace.SkierEvent as se on rd.id = se.raceDataId
            join hurace.TimeData as td on se.id = td.skierEventId
            join hurace.Sensor as s on s.id = td.sensorId
            where s.sensorNumber = 0 and rd.eventTypeId = 8 and td.skierId = @sid and td.raceId = @rid",
                                        queryParams: new QueryParam[] {("@sid", skierId), ("@rid", raceId)}))
            .FirstOrDefault()?.EventDateTime;

        public async Task<IEnumerable<TimeData>> GetRankingForSkier(int skierId)
        {
            return Enumerable.Empty<TimeData>();
        }
    }
}