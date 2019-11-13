using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Core.Dto
{
    public class RaceState: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string RaceStateDescription { get; set; } = default!;
    }
}