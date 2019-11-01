using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class RaceState
    {
        [Key]
        public int Id { get; set; }
        public string RaceStateDescription { get; set; }
    }
}