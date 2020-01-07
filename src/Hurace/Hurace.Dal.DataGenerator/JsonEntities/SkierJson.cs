using System.Diagnostics.CodeAnalysis;

namespace Hurace.DataGenerator.JsonEntities
{
    [ExcludeFromCodeCoverage]
    public class SkierJson
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }
    }
}