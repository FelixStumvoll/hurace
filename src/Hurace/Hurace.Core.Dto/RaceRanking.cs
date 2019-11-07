using System;
using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class RaceRanking
    {
        public int SkierId { get; set; }
        public int RaceId { get; set; }
        [Navigational] public Skier? Skier { get; set; }
        [Navigational] public StartList? StartList { get; set; }
        public DateTime RaceTime { get; set; }
    }
}