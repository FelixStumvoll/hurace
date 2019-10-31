using System;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceDataDao : DefaultDeleteBaseDao<RaceData>, IRaceDataDao
    {
        public RaceDataDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.RaceData", queryFactory)
        {
        }

        public override async Task<bool> UpdateAsync(RaceData obj) =>
            await GeneratedExecutionAsync(QueryFactory
                                              .Update<RaceData>()
                                              .Where(("id", obj.Id))
                                              .Build(obj, "Id"));

        public async Task<bool> InsertRaceEventAsync(int raceId, Events.RaceEvent raceEvent, DateTime dateTime) =>
            await InsertAsync(new RaceData {RaceId = raceId, EventTypeId = (int) raceEvent, EventDateTime = dateTime});

        public async Task<bool> InsertSkierEventAsync(int raceId, int skierId, Events.SkierEvent skierEvent,
            DateTime dateTime)
        {
            var raceDataId = await InsertGetIdAsync(new RaceData
            {
                RaceId = raceId, EventDateTime = dateTime, EventTypeId = (int) skierEvent
            });
            return (await ExecuteAsync("insert into hurace.SkierEvent values(@si, @rdi, @ri)",
                                       ("@si", skierId),
                                       ("@rdi", raceDataId),
                                       ("@ri", raceId))) == 1;
        }
    }
}