using Hurace.Core.Dto.Attributes;


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