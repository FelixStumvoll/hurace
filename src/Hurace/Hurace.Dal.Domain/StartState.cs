using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class StartState: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string StartStateDescription { get; set; } = default!;
    }
}