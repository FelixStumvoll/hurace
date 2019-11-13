using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Common.QueryBuilder.ConcreteQueryBuilder
{
    public class SelectStatementBuilder<T> : QueryBuilder<T> where T : class, new()
    {
        private JoinConfig? JoinCfg { get; set; }

        private class JoinConfig
        {
            public Dictionary<(Type selfType, Type foreignType), List<JoinParam>> JoinMappings { get; set; } =
                new Dictionary<(Type selfType, Type foreignType), List<JoinParam>>();

            public MapperConfig MapperConfig { get; set; } = new MapperConfig();
        }

        public SelectStatementBuilder(string schemaName) : base(schemaName)
        {
        }

        public SelectStatementBuilder<T> Join<TSelf, TRef>(params JoinParam[] mappings) where TRef : class, new()
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

        public SelectStatementBuilder<T> Where<TWhere>(params QueryParam[] where)
        {
            AddWhere<TWhere>(where);
            return this;
        }

        public (string statement, MapperConfig mapperConfig, IEnumerable<QueryParam> queryParams) Build()
        {
            var config = JoinCfg?.MapperConfig ?? new MapperConfig();
            var (whereSection, queryParams) = HandleWhere();
            return ($"select {HandleColumns(config)}{HandleJoins()}{whereSection}", config, queryParams);
        }

        private void AppendColumns<TSelect>(ICollection<string> list, MapperConfig config) where TSelect : class, new()
        {
            var tableName = WithSchema(typeof(TSelect).Name);
            foreach (var propertyInfo in typeof(TSelect).GetProperties())
            {
                if (Attribute.IsDefined(propertyInfo, typeof(NavigationalAttribute)))
                {
                    if (config.IsIncluded(propertyInfo.PropertyType))
                        typeof(SelectStatementBuilder<T>)
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
        
        private string HandleColumns(MapperConfig config)
        {
            var tableName = WithSchema(typeof(T).Name);
            var columnList = new List<string>();
            AppendColumns<T>(columnList, config);
            return $"{string.Join(',', columnList)} from {tableName}";
        }

        private string HandleJoins()
        {
            if (JoinCfg == null) return "";
            var joins = new List<string>();
            foreach (var ((selfTable, refTable), joinConstraints) in JoinCfg.JoinMappings)
            {
                var selfTableName = WithSchema(selfTable.Name);
                var refTableName = WithSchema(refTable.Name);
                var constraints =
                    string.Join(
                        " and ",
                        joinConstraints.Select(
                            jp => $"{selfTableName}.{jp.SelfColumn} = {refTableName}.{jp.ForeignColumn}"));
                joins.Add(
                    $"join {WithSchema(refTable.Name)} on {constraints}");
            }

            return $" {string.Join(" ", joins)}";
        }
    }
}