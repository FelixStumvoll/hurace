using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}