using System;
using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Attributes;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Core.Dto
{
    public class RaceData: ISinglePkEntity
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