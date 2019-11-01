using System;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class TimeData
    {
        public DateTime Time { get; set; }
        [Key]
        public int SkierId { get; set; }
        [Navigational]
        public Skier? Skier { get; set; }
        public int RaceDataId { get; set; }
        [Key]
        public int RaceId { get; set; }
        [Navigational]
        public Race? Race { get; set; }
        [Key]
        public int SensorId { get; set; }
        [Navigational]
        public Sensor? Sensor { get; set; }
    }
}