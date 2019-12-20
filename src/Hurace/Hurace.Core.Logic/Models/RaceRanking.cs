using Hurace.Dal.Domain;

namespace Hurace.Core.Logic.Models
{
    public class RaceRanking
    {
        public StartList StartList { get; set; }
        public int? Time { get; set; }
        public int? Position { get; set; }
        public int? TimeToLeader { get; set; }
        public bool Disqualified => !Position.HasValue;

        public RaceRanking(StartList startList, int? time = null, int? position = null, int? timeToLeader = null)
        {
            StartList = startList;
            Time = time;
            Position = position;
            TimeToLeader = timeToLeader;
        }
    }
}