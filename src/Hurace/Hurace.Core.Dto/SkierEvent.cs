using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Attributes;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Core.Dto
{
    public class SkierEvent: ISinglePkEntity
    {
        [Key] public int Id { get; set; }
        [Navigational] public StartList? StartList { get; set; }
        public int SkierId { get; set; }
        public int RaceId { get; set; }
        public int RaceDataId { get; set; }
        [Navigational] public RaceData? RaceData { get; set; }
    }
}