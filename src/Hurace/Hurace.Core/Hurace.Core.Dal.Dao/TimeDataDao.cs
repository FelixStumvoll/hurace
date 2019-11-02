using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class TimeDataDao : BaseDao<TimeData>, ITimeDataDao
    {
        public TimeDataDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.TimeData", statementFactory)
        {
        }

        public override async Task<bool> InsertAsync(TimeData obj) =>
            await GeneratedExecutionAsync(StatementFactory.Insert<TimeData>().WithKey().Build(obj));

        public override async Task<int> InsertGetIdAsync(TimeData obj) =>
            await GeneratedExecutionGetIdAsync(StatementFactory.Insert<TimeData>().WithKey().Build(obj));

        public Task<IEnumerable<TimeData>> GetRankingForRace(int raceId) =>
            QueryAsync<TimeData>(@"select	s.id,
                                                s.lastName,
                                                s.dateOfBirth,
                                                s.countryId,
                                                s.genderId,
                                                s.firstName,
                                                c.countryCode,
                                                c.countryName,
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
                                                c.countryName
                                                order by raceTime asc",
                                 new MapperConfig()
                                     .AddMapping<Country>(("countryId", "Id"))
                                     .Include<Skier>()
                                     .Include<Country>(),
                                 ("@id", raceId));

        public async Task<bool> DeleteAsync(int skierId, int raceId, int sensorId) =>
            await ExecuteAsync($"delete from {TableName} where skierId=@sId and raceId=@rId and sensorId=@sensorId",
                                ("@sid", skierId), ("@rId", raceId), ("@sensorId", sensorId));

        protected override SelectStatementBuilder<TimeData> DefaultSelectQuery() =>
            StatementFactory
                .Select<TimeData>()
                .Join<TimeData, StartList>(("skierId", "skierId"), ("raceId", "raceId"))
                .Join<TimeData, Sensor>(("sensorId", "id"));

        public async Task<TimeData> FindByIdAsync(int skierId, int raceId, int sensorId) =>
            (await GeneratedQueryAsync(DefaultSelectQuery()
                                       .Where<TimeData>(("skierId", skierId), 
                                                        ("raceId", raceId),
                                                        ("sensorId", sensorId))
                                       .Build()))
            .SingleOrDefault();
    }
}