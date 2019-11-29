using System;
using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Attributes;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
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