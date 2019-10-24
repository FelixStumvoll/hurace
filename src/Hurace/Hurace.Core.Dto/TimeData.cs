using System;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class TimeData
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int SkierId { get; set; }
        [Navigational]
        public Skier Skier { get; set; }
        public int RaceDataId { get; set; }
    }
}