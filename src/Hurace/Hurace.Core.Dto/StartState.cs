using Hurace.Core.Dto.Attributes;


namespace Hurace.Core.Dto
{
    public class StartState
    {
        [Key]
        public int Id { get; set; }

        public string StartStateDescription { get; set; } = default!;
    }
}