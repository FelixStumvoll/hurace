using System.Diagnostics.CodeAnalysis;

namespace Hurace.Dal.DataGenerator.Core.JsonEntities
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