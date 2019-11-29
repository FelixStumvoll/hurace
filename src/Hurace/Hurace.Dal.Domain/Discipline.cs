using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class Discipline: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string DisciplineName { get; set; } = default!;
    }
}