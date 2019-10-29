using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Common;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public class UpdateQueryBuilder<T> : QueryBuilder<T> where T : class, new()
    {
        public UpdateQueryBuilder(string schemaName) : base(schemaName)
        {
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

            var (whereSection, whereQueryParams) = HandleWhere();
            queryParams.AddRange(whereQueryParams);
            return ($"update {WithSchema(typeof(T).Name)} set {string.Join(',', updateProps)} where" +
                    $" {whereSection}", queryParams);
        }

        public UpdateQueryBuilder<T> Where(params QueryParam[] whereParams)
        {
             AddWhere<T>(whereParams);
             return this;
        }
    }
}