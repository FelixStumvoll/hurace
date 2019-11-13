using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Interfaces;


namespace Hurace.Core.Dto
{
    public class Gender: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }
        public string GenderDescription { get; set; } = default!;
    }
}