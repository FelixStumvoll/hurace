using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class RaceState
    {
        [Key]
        public int Id { get; set; }

        public string RaceStateDescription { get; set; } = default!;
    }
}