using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class StartList
    {
        [Key]
        public int RaceId { get; set; }
        [Navigational]
        public Race? Race { get; set; }
        [Key]
        public int SkierId { get; set; }
        [Navigational]
        public Skier? Skier { get; set; }
        public int StartNumber { get; set; }
        public int StartStateId { get; set; }
        [Navigational]
        public StartState? StartState { get; set; }
    }
}