using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class EventType
    {
        [Key]
        public int Id { get; set; }
        public string EventDescription { get; set; }
    }
}