using System;
using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class RaceData
    {
        [Key]
        public int Id { get; set; }
        public int RaceId { get; set; }
        public DateTime EventDateTime { get; set; }
        public int EventTypeId { get; set; }
        [Navigational]
        public EventType? EventType { get; set; }
    }
}