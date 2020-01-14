using System.Diagnostics.CodeAnalysis;

namespace Hurace.Dal.DataGenerator.Core.JsonEntities
{
    [ExcludeFromCodeCoverage]
    public class SkierJson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderId { get; set; }
        public string CountryCode { get; set; }
        public string ImageUrl { get; set; }
    }
}