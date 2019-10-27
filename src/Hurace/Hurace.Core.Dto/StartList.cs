using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class StartList
    {
        public int RaceId { get; set; }
//        [Navigational]
//        public Race Race { get; set; }
        public int SkierId { get; set; }
        [Navigational]
        public Skier Skier { get; set; }
        public int StartNumber { get; set; }
        public int StartStateId { get; set; }
        [Navigational]
        public StartState StartState { get; set; }
    }
}