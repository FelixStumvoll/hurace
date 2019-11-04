using Hurace.Core.Dto.Attributes;


namespace Hurace.Core.Dto
{
    public class Discipline
    {
        [Key]
        public int Id { get; set; }
        public string DisciplineName { get; set; }
    }
}