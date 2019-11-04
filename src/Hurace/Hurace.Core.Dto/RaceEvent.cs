using Hurace.Core.Dto.Attributes;


namespace Hurace.Core.Dto
{
    public class RaceEvent
    {
        [Key]
        public int Id { get; set; }
        public int RaceDataId { get; set; }
        [Navigational]
        public RaceData RaceData { get; set; }
    }
}