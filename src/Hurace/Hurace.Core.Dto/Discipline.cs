using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Interfaces;


namespace Hurace.Core.Dto
{
    public class Discipline: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string DisciplineName { get; set; } = default!;
    }
}