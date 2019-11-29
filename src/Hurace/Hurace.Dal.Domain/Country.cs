using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class Country : ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string CountryName { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
    }
}