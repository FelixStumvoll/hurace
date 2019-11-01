using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Discipline
    {
        [Key]
        public int Id { get; set; }
        public string DisciplineName { get; set; }
    }
}