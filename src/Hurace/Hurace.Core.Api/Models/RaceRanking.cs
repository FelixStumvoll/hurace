using Hurace.Dal.Domain;

namespace Hurace.Core.Api.Models
{
    public class RaceRanking
    {
        public StartList StartList { get; set; }
        public int? Time { get; set; }
        public int? Position { get; set; }
        public int? TimeToLeader { get; set; }
        public bool Disqualified => !Position.HasValue;
    }
}