using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Interfaces;


namespace Hurace.Core.Dto
{
    public class Country : ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string CountryName { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
    }
}