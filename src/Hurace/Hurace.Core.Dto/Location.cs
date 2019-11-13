using Hurace.Core.Dto.Attributes;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Core.Dto
{
    public class Location: ISinglePkEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        public string LocationName { get; set; } = default!;
        public int CountryId { get; set; }
        [Navigational]
        public Country? Country { get; set; }
    }
}