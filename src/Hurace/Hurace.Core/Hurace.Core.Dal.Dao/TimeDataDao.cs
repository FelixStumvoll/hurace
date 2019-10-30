using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class TimeDataDao : BaseDao<TimeData>, ITimeDataDao
    {
        public TimeDataDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.timedata", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(TimeData obj) =>
            await GeneratedExecutionAsync(QueryFactory.Update<TimeData>()
                                              .Where(("skierId", obj.SkierId), ("raceId", obj.RaceId))
                                              .Build(obj, "SkierId", "RaceId"));

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
                                                order by raceTime desc",
                                 new MapperConfig()
                                     .AddMapping<Country>(("countryId", "Id"))
                                     .Include<Skier>()
                                     .Include<Country>(),
                                 ("@id", raceId));

        public async Task<bool> DeleteAsync(int skierId, int raceId, int sensorId) =>
            (await ExecuteAsync($"delete from {TableName} where skierId=@sId and raceId=@rId and sensorId=@sensorId",
                                ("@sid", skierId), ("@rId", raceId), ("@sensorId", sensorId))) == 1;
    }
}