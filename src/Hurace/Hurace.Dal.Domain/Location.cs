using Hurace.Dal.Domain.Attributes;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
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