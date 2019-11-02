using System;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class TimeData
    {
        [Key] public int SkierId { get; set; }
        [Key] public int RaceId { get; set; }
        [Key] public int SensorId { get; set; }
        public DateTime Time { get; set; }
        public int SkierEventId { get; set; }
        [Navigational] public Skier? Skier { get; set; }
        [Navigational] public SkierEvent? SkierEvent { get; set; }
        [Navigational] public Race? Race { get; set; }
        [Navigational] public Sensor? Sensor { get; set; }
    }
}