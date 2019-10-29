using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Hurace.Core.Common;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public class SelectQueryBuilder<T> : QueryBuilder where T : class, new()
    {
        private JoinConfig JoinCfg { get; set; }

        private class JoinConfig
        {
            public Dictionary<(Type selfType, Type foreignType), List<JoinParam>> JoinMappings { get; set; } = 
                new Dictionary<(Type selfType, Type foreignType), List<JoinParam>>();

            public MapperConfig MapperConfig { get; set; } = new MapperConfig();
        }

        private readonly List<(string tableName, IEnumerable<QueryParam> param)> _whereConditions =
            new List<(string tabelName, IEnumerable<QueryParam> param)>();

        public SelectQueryBuilder(string schemaName) : base(schemaName)
        {
        }

        public SelectQueryBuilder<T> Join<TSelf, TRef>(params JoinParam[] mappings) where TRef : class, new()
        {
            JoinCfg ??= new JoinConfig();
            if (!JoinCfg.JoinMappings.TryGetValue((typeof(TSelf), typeof(TRef)), out var list))
            {
                list = new List<JoinParam>();
                JoinCfg.JoinMappings.Add((typeof(TSelf), typeof(TRef)), list);
            }

            list.AddRange(mappings);
            JoinCfg.MapperConfig.Include<TRef>();
            return this;
        }

        public SelectQueryBuilder<T> Where<TWhere>(params QueryParam[] where)
        {
            _whereConditions.Add((WithSchema(typeof(TWhere).Name), where));
            return this;
        }

        public (string statement, MapperConfig mapperConfig, IEnumerable<QueryParam> queryParams) Build()
        {
            var config = JoinCfg?.MapperConfig ?? new MapperConfig();
            var strBuilder = new StringBuilder("Select ");
            HandleColumns(strBuilder, config);
            HandleJoins(strBuilder);
            return (strBuilder.ToString(), config, HandleWhere(strBuilder));
        }

        private void AppendColumns<TSelect>(ICollection<string> list, MapperConfig config) where TSelect : class, new()
        {
            var tableName = WithSchema(typeof(TSelect).Name);
            foreach (var propertyInfo in typeof(TSelect).GetProperties())
            {
                if (Attribute.IsDefined(propertyInfo, typeof(NavigationalAttribute)))
                {
                    if (config.IsIncluded(propertyInfo.PropertyType))
                        typeof(SelectQueryBuilder<T>)
                            .GetMethod(nameof(AppendColumns), BindingFlags.NonPublic | BindingFlags.Instance)
                            ?.MakeGenericMethod(propertyInfo.PropertyType)
                            .Invoke(this, new object[] {list, config});
                    continue;
                }

                var alias = $"{typeof(TSelect).Name}_{propertyInfo.Name}";
                list.Add($"{tableName}.{propertyInfo.Name.ToLowerFirstChar()} as {alias}");
                config.AddMapping<TSelect>((alias, propertyInfo.Name));
            }
        }


        private void HandleColumns(StringBuilder strBuilder, MapperConfig config)
        {
            var tableName = WithSchema(typeof(T).Name);
            var columnList = new List<string>();
            AppendColumns<T>(columnList, config);
            strBuilder.Append($"{string.Join(',', columnList)} from {tableName}");
        }

        private void HandleJoins(StringBuilder strBuilder)
        {
            if (JoinCfg == null) return;
            foreach (var ((selfTable, refTable), joinConstraints) in JoinCfg.JoinMappings)
            {
                var joinList = new List<string>();
                var selfTableName = WithSchema(selfTable.Name);
                var refTableName = WithSchema(refTable.Name);
                strBuilder.Append($" join {refTableName} on ");

                joinConstraints.ForEach(cm => joinList.Add(
                                            $"{selfTableName}.{cm.SelfColumn} = {refTableName}.{cm.ForeignColumn}"));

                strBuilder.Append(string.Join(" and ", joinList));
            }
        }

        private IEnumerable<QueryParam> HandleWhere(StringBuilder stringBuilder)
        {
            var queryParams = new List<QueryParam>();
            if (_whereConditions.Count == 0) return queryParams;
            stringBuilder.Append(" where ");
            var whereList = new List<string>();
            foreach (var (tableName, whereColumns) in _whereConditions)
            foreach (var whereQueryParam in whereColumns)
            {
                whereList.Add($"{tableName}.{whereQueryParam.Name}=@{whereQueryParam.Name}");
                queryParams.Add(($"@{whereQueryParam.Name}",whereQueryParam.Value));
            }

            stringBuilder.Append(string.Join(" and ", whereList));
            return queryParams;
        }
    }
}