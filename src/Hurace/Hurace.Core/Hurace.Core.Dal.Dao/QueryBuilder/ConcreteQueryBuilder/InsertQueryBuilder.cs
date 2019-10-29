using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Common;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public class InsertQueryBuilder<T> : AbstractQueryBuilder
    {
        public InsertQueryBuilder(string schemaName) : base(schemaName)
        {
        }

        public (string statement, IEnumerable<QueryParam> queryParams) Build(T obj, params string[] excludedProperties)
        {
            var queryParams = new List<QueryParam>();
            var columnNames = new List<string>();
            var columnValues = new List<string>();
            var properties = GetCrudProperties(obj).ToList();

            properties.Where(pi => !excludedProperties.Contains(pi.name)).ToList().ForEach(pi =>
            {
                var (name, value) = pi;
                columnNames.Add(name);
                columnValues.Add($"@{name}");
                queryParams.Add(($"@{name}", value));
            });

            return (
                $"insert into {WithSchema(typeof(T).Name)} ({string.Join(',', columnNames)})" +
                $" values ({string.Join(',', columnValues)})", queryParams);
        }
    }
}