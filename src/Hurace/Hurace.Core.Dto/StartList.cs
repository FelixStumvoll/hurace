namespace Hurace.Core.Dto
{
    public class StartList
    {
        public int RaceId { get; set; }
        public int SkierId { get; set; }
        public int StartNumber { get; set; }
        public int StartStateId { get; set; }
        public StartState StartState { get; set; }
    }
}