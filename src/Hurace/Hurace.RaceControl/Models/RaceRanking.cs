using Hurace.Dal.Domain;

namespace Hurace.RaceControl.Models
{
    public class RaceRanking
    {
        public StartList StartList { get; set; }
        public int? Time { get; set; }
        public bool Disqualified => !Time.HasValue;
    }
}