using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class RaceData
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public int EventTypeId { get; set; }
        [Navigational]
        public EventType EventType { get; set; }
    }
}