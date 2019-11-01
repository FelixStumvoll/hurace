using System;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceDataDao : BaseDao<RaceData>, IRaceDataDao
    {
        public RaceDataDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.RaceData", statementFactory)
        {
        }

        #region RaceEvent

        public async Task<bool> InsertRaceEventAsync(int raceId, Constants.RaceEvent raceEvent, DateTime dateTime) =>
            await InsertAsync(new RaceData {RaceId = raceId, EventTypeId = (int) raceEvent, EventDateTime = dateTime});

        public async Task<int>
            InsertRaceEventGetIdAsync(int raceId, Constants.RaceEvent raceEvent, DateTime dateTime) =>
            await InsertGetIdAsync(new RaceData
                                       {RaceId = raceId, EventTypeId = (int) raceEvent, EventDateTime = dateTime});

        public async Task<bool> DeleteRaceEvent(int raceDataId) =>
            await DeleteRaceData(raceDataId, 1);

        #endregion

        private async Task<bool> DeleteRaceData(int raceDataId, int eventSuperType) =>
            await ExecuteAsync(
                $@"delete rd from hurace.RaceData as rd 
                                join hurace.EventType as et on et.id = rd.eventTypeId 
                                where eventSuperType = {eventSuperType} and rd.id = @id", ("@id", raceDataId));

        #region SkierEvent

        private async Task<int> InsertRaceDataGetId(int raceId, DateTime dateTime, int eventType) =>
            await InsertGetIdAsync(new RaceData
            {
                RaceId = raceId, EventDateTime = dateTime, EventTypeId = eventType
            });

        private async Task<bool> InsertSkierEventForRaceData(int skierId, int raceDataId, int raceId) =>
            await ExecuteAsync("insert into hurace.SkierEvent values(@si, @rdi, @ri)",
                                ("@si", skierId),
                                ("@rdi", raceDataId),
                                ("@ri", raceId));

        public async Task<bool> InsertSkierEventAsync(int raceId, int skierId, Constants.SkierEvent skierEvent,
            DateTime dateTime)
        {
            var raceDataId = await InsertRaceDataGetId(raceId, dateTime, (int) skierEvent);
            return await InsertSkierEventForRaceData(skierId, raceDataId, raceId);
        }

        public async Task<int> InsertSkierEventGetIdAsync(int raceId, int skierId, Constants.SkierEvent skierEvent,
            DateTime dateTime)
        {
            var raceDataId = await InsertRaceDataGetId(raceId, dateTime, (int) skierEvent);
            await InsertSkierEventForRaceData(skierId, raceDataId, raceId);
            return raceDataId;
        }

        public async Task<bool> DeleteSkierEvent(int raceDataId)
        {
            await ExecuteAsync("delete from hurace.SkierEvent where raceDataId = @rdi", ("@rdi", raceDataId));
            return await DeleteRaceData(raceDataId, 2);
        }

        public Task<bool> AddSkierToRaceData(int raceDataId, int skierId, int raceId)
        {
//            ExecuteAsync("insert into hurace.")
            return Task.FromResult(true);
        }

        #endregion
    }
}