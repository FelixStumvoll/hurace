using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class RaceDataSkier : RaceData
    {
        public int SkierId { get; set; }
        [Navigational]
        public Skier? Skier { get; set; }
    }
}