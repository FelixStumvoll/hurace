using System.ComponentModel.DataAnnotations;

namespace Hurace.Core.Dto.Interfaces
{
    public interface ISinglePkEntity
    {
        [Key]
        int Id { get; set; }
    }
}