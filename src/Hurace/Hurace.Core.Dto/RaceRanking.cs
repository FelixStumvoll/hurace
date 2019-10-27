using System;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class RaceRanking
    {
        [Navigational]
        public Skier? Skier { get; set; }
        public DateTime RaceTime { get; set; }
    }
}