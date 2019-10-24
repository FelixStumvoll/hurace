namespace Hurace.Core.Common
{
    public class QueryParam
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public QueryParam(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public static implicit operator QueryParam((string name, object value) tuple) =>
            new QueryParam(tuple.name, tuple.value);
    }
}