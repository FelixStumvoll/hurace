using System.ComponentModel.DataAnnotations;

namespace Hurace.Dal.Domain.Interfaces
{
    public interface ISinglePkEntity
    {
        [Key]
        int Id { get; set; }
    }
}