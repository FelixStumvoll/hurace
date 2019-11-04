using Hurace.Core.Dto.Attributes;


namespace Hurace.Core.Dto
{
    public class EventType
    {
        [Key]
        public int Id { get; set; }
        public string EventDescription { get; set; }
    }
}