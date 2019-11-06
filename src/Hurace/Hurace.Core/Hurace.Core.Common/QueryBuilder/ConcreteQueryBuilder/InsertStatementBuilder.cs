using System.Collections.Generic;
using System.Linq;

namespace Hurace.Core.Common.QueryBuilder.ConcreteQueryBuilder
{
    public class InsertStatementBuilder<T> : AbstractStatementBuilder
    {
        public InsertStatementBuilder(string schemaName) : base(schemaName)
        {
        }

        private bool _withKey;

        public InsertStatementBuilder<T> WithKey()
        {
            _withKey = true;
            return this;
        }

        public (string statement, IEnumerable<QueryParam> queryParams) Build(T obj)
        {
            var queryParams = new List<QueryParam>();
            var columnNames = new List<string>();
            var columnValues = new List<string>();
            var properties = GetNonNavigationalProps(obj,_withKey).ToList();

            properties
                .ToList().ForEach(pi =>
            {
                var (name, value) = pi;
                columnNames.Add(name);
                columnValues.Add(AddParamSymbol(name));
                queryParams.Add((AddParamSymbol(name), value));
            });

            return (
                $"insert into {WithSchema(typeof(T).Name)} ({string.Join(',', columnNames)})" +
                $" values ({string.Join(',', columnValues)})", queryParams);
        }
    }
}