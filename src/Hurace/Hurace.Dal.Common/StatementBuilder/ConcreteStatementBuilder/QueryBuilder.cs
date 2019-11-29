using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder
{
    public abstract class QueryBuilder<T> : AbstractStatementBuilder where T : class, new()
    {
        protected WhereConfig? WhereCfg { get; private set; }

        protected class WhereConfig
        {
            public List<(string tableName, IEnumerable<QueryParam>)> WhereConditions { get; } =
                new List<(string tableName, IEnumerable<QueryParam>)>();
        }

        protected QueryBuilder(string schemaName) : base(schemaName)
        {
        }


        protected void AddWhere<TWhere>(params QueryParam[] whereParams)
        {
            WhereCfg ??= new WhereConfig();
            WhereCfg.WhereConditions.Add((WithSchema(typeof(TWhere).Name), whereParams));
        }

        protected void AddWhereId(T obj)
        {
            WhereCfg ??= new WhereConfig();
            WhereCfg.WhereConditions.Add((WithSchema(typeof(T).Name), obj switch
            {
                ISinglePkEntity spe => new QueryParam[] {(nameof(ISinglePkEntity.Id), spe.Id)},
                _ => typeof(T).GetProperties()
                              .Where(pi => Attribute.IsDefined(pi, typeof(KeyAttribute)) && pi.GetValue(obj) != null)
                              .Select(pi => new QueryParam (pi.Name, pi.GetValue(obj)!))
            }));
        }

        protected (string statement, IEnumerable<QueryParam> queryParams) HandleWhere()
        {
            var queryParams = new List<QueryParam>();
            if (WhereCfg == null) return ("", queryParams);
            var whereConditions = new List<string>();
            foreach (var (tableName, whereQueryParams) in WhereCfg.WhereConditions)
            foreach (var whereQueryParam in whereQueryParams)
            {
                whereConditions.Add($"{tableName}.{whereQueryParam.Name}=@WHERE{whereQueryParam.Name}");
                whereQueryParam.Name = $"WHERE{whereQueryParam.Name}";
                queryParams.Add(AddParamSymbol(whereQueryParam));
            }

            return ($" where {string.Join(" and ", whereConditions)}", queryParams);
        }
    }
}