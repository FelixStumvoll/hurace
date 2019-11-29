using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Attributes;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class RaceEvent: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }
        public int RaceDataId { get; set; }
        [Navigational]
        public RaceData? RaceData { get; set; }
    }
}