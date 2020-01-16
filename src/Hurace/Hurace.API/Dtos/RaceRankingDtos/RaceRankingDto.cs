using Hurace.Core.Interface.Entities;
using Hurace.Dal.Domain;

namespace Hurace.API.Dtos.RaceRankingDtos
{
    public class RaceRankingDto
    {
        public int? Time { get; set; }
        public int? Position { get; set; }
        public int? TimeToLeader { get; set; }
        public bool Disqualified { get; set; }
        public int StartNumber { get; set; }
        public Skier Skier { get; set; }
        
        public static RaceRankingDto FromRaceRanking(RaceRanking ranking) => new RaceRankingDto
        {
            Time = ranking.Time,
            Position = ranking.Position,
            Disqualified = ranking.Disqualified,
            StartNumber = ranking.StartList.StartNumber,
            Skier = ranking.StartList.Skier,
            TimeToLeader = ranking.TimeToLeader
        };
    }
}