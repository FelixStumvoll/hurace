using System;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dto;

namespace Hurace.Dal.Interface
{
    public interface IRaceDataDao : IDefaultDeleteBaseDao<RaceData>
    {
        Task<bool> InsertRaceEventAsync(int raceId, Events.RaceEvent raceEvent, DateTime dateTime);
        Task<bool> InsertSkierEventAsync(int raceId,int skierId, Events.SkierEvent skierEvent, DateTime dateTime);
    }
}