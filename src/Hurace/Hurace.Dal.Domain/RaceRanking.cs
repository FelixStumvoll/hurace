using System;
using Hurace.Dal.Domain.Attributes;

namespace Hurace.Dal.Domain
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