using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int CountryId { get; set; }
        [Navigational]
        public Country? Country { get; set; }
    }
}