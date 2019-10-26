namespace Hurace.Core.Common.Mapper
{
    public class PropertyMapping
    {
        public string SrcName { get; set; }
        public string DestName { get; set; }

        public static implicit operator PropertyMapping((string srcName, string destName) mapping) =>
            new PropertyMapping
            {
                SrcName = mapping.srcName,
                DestName = mapping.destName
            };
    }
}