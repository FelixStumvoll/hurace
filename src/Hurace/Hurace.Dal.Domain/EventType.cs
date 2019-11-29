using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class EventType: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string EventDescription { get; set; } = default!;
    }
}