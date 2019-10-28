using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Common;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public class InsertQueryBuilder<T> : QueryBuilder
    {
        public InsertQueryBuilder(string schemaName) : base(schemaName)
        {
        }

        public (string statement, IEnumerable<QueryParam> queryParams) Build(T obj)
        {
            var queryParams = new List<QueryParam>();
            var columnNames = new List<string>();
            var columnValues = new List<string>();
            var properties = GetCrudProperties(obj).ToList();

            properties.ForEach(pi =>
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