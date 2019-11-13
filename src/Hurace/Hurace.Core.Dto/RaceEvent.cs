using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Attributes;
using Hurace.Core.Dto.Interfaces;


namespace Hurace.Core.Dto
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