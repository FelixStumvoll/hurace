using System.Collections.Generic;
using Hurace.Core.Common;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public abstract class QueryBuilder<T> : AbstractQueryBuilder where T : class, new()
    {
        protected WhereConfig WhereCfg { get; private set; }

        protected class WhereConfig
        {
            public List<(string tableName, IEnumerable<QueryParam>)> WhereConditions { get; } =
                new List<(string tableName, IEnumerable<QueryParam>)>();
        }

        protected QueryBuilder(string schemaName) : base(schemaName)
        {
        }

        private static QueryParam AddParamSymbol(QueryParam queryParam)
        {
            queryParam.Name = $"@{queryParam.Name}";
            return queryParam;
        }

        protected void AddWhere<TWhere>(params QueryParam[] whereParams)
        {
            WhereCfg ??= new WhereConfig();
            WhereCfg.WhereConditions.Add((WithSchema(typeof(TWhere).Name), whereParams));
        }

        protected (string statement, IEnumerable<QueryParam> queryParams) HandleWhere()
        {
            var queryParams = new List<QueryParam>();
            if (WhereCfg == null) return ("", queryParams);
            var whereConditions = new List<string>();
            foreach (var (tableName, whereQueryParams) in WhereCfg.WhereConditions)
            foreach (var whereQueryParam in whereQueryParams)
            {
                whereConditions.Add($"{tableName}.{whereQueryParam.Name}=@{whereQueryParam.Name}");
                queryParams.Add(AddParamSymbol(whereQueryParam));
            }

            return ($" where {string.Join(" and ", whereConditions)}", queryParams);
        }
    }
}