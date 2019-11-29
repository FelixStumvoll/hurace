using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class Gender: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }
        public string GenderDescription { get; set; } = default!;
    }
}