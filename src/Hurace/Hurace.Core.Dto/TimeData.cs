using System;
using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class TimeData
    {
        [Key] public int SkierId { get; set; }
        [Key] public int RaceId { get; set; }
        [Key] public int SensorId { get; set; }
        public DateTime Time { get; set; }
        public int SkierEventId { get; set; }
        [Navigational] public SkierEvent? SkierEvent { get; set; }
        [Navigational] public Sensor? Sensor { get; set; }
        [Navigational] public StartList? StartList { get; set; }
    }
}