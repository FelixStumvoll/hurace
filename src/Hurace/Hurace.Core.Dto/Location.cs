using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        [Navigational]
        public Country Country { get; set; }
    }
}