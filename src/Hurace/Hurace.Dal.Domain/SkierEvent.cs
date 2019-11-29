using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Attributes;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
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