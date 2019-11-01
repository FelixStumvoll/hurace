using System;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface IRaceDataDao : IBaseDao<RaceData>
    {
        #region RaceEvent
        Task<bool> InsertRaceEventAsync(int raceId, Constants.RaceEvent raceEvent, DateTime dateTime);
        Task<int> InsertRaceEventGetIdAsync(int raceId, Constants.RaceEvent raceEvent, DateTime dateTime);
        Task<bool> DeleteRaceEvent(int raceDataId);
        #endregion

        #region SkierEvent

        Task<bool> InsertSkierEventAsync(int raceId, int skierId, Constants.SkierEvent skierEvent, DateTime dateTime);

        Task<int> InsertSkierEventGetIdAsync(int raceId, int skierId, Constants.SkierEvent skierEvent,
            DateTime dateTime);

        Task<bool> DeleteSkierEvent(int raceDataId);
        Task<bool> AddSkierToRaceData(int raceDataId,int skierId, int raceId);

        #endregion
    }
}