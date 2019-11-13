using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Interfaces;


namespace Hurace.Core.Dto
{
    public class EventType: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string EventDescription { get; set; } = default!;
    }
}