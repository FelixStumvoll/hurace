using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Sensor
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int RaceId { get; set; }
        [Navigational]
        public Race? Race { get; set; }
    }
}