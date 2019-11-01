using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Sensor
    {
        [Key]
        public int Id { get; set; }
        public string? SensorDescription { get; set; }
        public int RaceId { get; set; }
        [Navigational]
        public Race? Race { get; set; }
    }
}