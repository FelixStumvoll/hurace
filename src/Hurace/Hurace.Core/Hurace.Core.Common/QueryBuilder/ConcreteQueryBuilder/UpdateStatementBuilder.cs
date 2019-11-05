using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Common;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public class UpdateStatementBuilder<T> : QueryBuilder<T> where T : class, new()
    {
        public UpdateStatementBuilder(string schemaName) : base(schemaName)
        {
        }

        private bool _withKey = false;

        public UpdateStatementBuilder<T> WithKey()
        {
            _withKey = true;
            return this;
        }
        
        public (string statement, IEnumerable<QueryParam> queryParams) Build(T obj)
        {
            var queryParams = new List<QueryParam>();
            var updateProps = new List<string>();

            GetNonNavigationalProps(obj, _withKey)
                .ToList()
                .ForEach(pi =>
                {
                    var (name, value) = pi;
                    updateProps.Add($"{name}=@{name}");
                    queryParams.Add(($"@{name}", value));
                });

            var (whereSection, whereQueryParams) = HandleWhere();
            queryParams.AddRange(whereQueryParams);
            return ($"update {WithSchema(typeof(T).Name)} set {string.Join(',', updateProps)}" +
                    $"{whereSection}", queryParams);
        }

        public UpdateStatementBuilder<T> Where(params QueryParam[] whereParams)
        {
            AddWhere<T>(whereParams);
            return this;
        }
        
        public UpdateStatementBuilder<T> WhereId(T obj)
        {
            AddWhereId(obj);
            return this;
        }
    }
}