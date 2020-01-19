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
        public Skier? Skier { get; set; }

        public RaceRankingDto(int? time, int? position, int? timeToLeader, bool disqualified, int startNumber,
            Skier? skier)
        {
            Time = time;
            Position = position;
            TimeToLeader = timeToLeader;
            Disqualified = disqualified;
            StartNumber = startNumber;
            Skier = skier;
        }

        public static RaceRankingDto FromRaceRanking(RaceRanking ranking) => new RaceRankingDto
        (
            ranking.Time,
            ranking.Position,
            ranking.TimeToLeader,
            ranking.Disqualified,
            ranking.StartList.StartNumber,
            ranking.StartList.Skier
        );
    }
}