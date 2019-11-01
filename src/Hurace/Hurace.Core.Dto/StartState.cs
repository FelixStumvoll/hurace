using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class StartState
    {
        [Key]
        public int Id { get; set; }
        public string StartStateDescription { get; set; }
    }
}