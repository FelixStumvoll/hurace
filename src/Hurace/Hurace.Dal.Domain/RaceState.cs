using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class RaceState: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string RaceStateDescription { get; set; } = default!;
    }
}