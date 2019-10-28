using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Common;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public class UpdateQueryBuilder<T> : QueryBuilder
    {
        private readonly List<QueryParam> _whereProperties = new List<QueryParam>();

        public UpdateQueryBuilder(string schemaName) : base(schemaName)
        {
        }

        public UpdateQueryBuilder<T> Where(params QueryParam[] whereProperties)
        {
            _whereProperties.AddRange(whereProperties);
            return this;
        }

        public (string statement, IEnumerable<QueryParam> queryParams) Build(T obj)
        {
            var queryParams = new List<QueryParam>();
            var updateProps = new List<string>();
            var properties = GetCrudProperties(obj).ToList();

            properties.ToList().ForEach(pi =>
            {
                var (name, value) = pi;
                updateProps.Add($"{name}=@{name}");
                queryParams.Add(($"@{name}", value));
            });

            queryParams.AddRange(AddParamSymbol(_whereProperties));

            var whereSection =
                string.Join(
                    "and",
                    _whereProperties
                        .Select(prop => $"{WithSchema(typeof(T).Name)}.{prop.Name}=@{prop.Name}"));
            return ($"update {WithSchema(typeof(T).Name)} set {string.Join(',', updateProps)} where" +
                    $" {whereSection}", queryParams);
        }
    }
}