namespace Hurace.Core.Common
{
    public class QueryParam
    {
        public string Name { get; set; }
        public object Value { get; set; }
        
        public static implicit operator QueryParam((string name, object value) tuple) =>
            new QueryParam{ Name = tuple.name, Value = tuple.value};
    }
}