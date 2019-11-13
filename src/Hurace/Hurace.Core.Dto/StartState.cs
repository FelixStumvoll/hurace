using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Interfaces;


namespace Hurace.Core.Dto
{
    public class StartState: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string StartStateDescription { get; set; } = default!;
    }
}