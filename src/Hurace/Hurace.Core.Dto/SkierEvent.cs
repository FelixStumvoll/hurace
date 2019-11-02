using System;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class SkierEvent
    {
        [Key] public int Id { get; set; }
        [Navigational] public StartList? StartList { get; set; }
        public int SkierId { get; set; }
        public int RaceId { get; set; }
        public int RaceDataId { get; set; }
        [Navigational] public RaceData? RaceData { get; set; }
    }
}