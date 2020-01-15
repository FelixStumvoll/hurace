using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hurace.Dal.Common.Mapper;
using Hurace.Dal.Domain.Attributes;

namespace Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder
{
    public class SelectStatementBuilder<T> : QueryBuilder<T> where T : class, new()
    {
        private JoinConfig? JoinCfg { get; set; }


        private class InnerJoinConfig
        {
            public List<JoinParam> JoinParams { get; set; } = new List<JoinParam>();
        }

        private class LeftOuterJoinConfig : InnerJoinConfig
        {
            public List<string> NullKeys { get; set; } = new List<string>();
        }

        private class JoinConfig
        {
            public Dictionary<(Type selfType, Type foreignType), InnerJoinConfig> JoinConfigs { get; set; } =
                new Dictionary<(Type selfType, Type foreignType), InnerJoinConfig>();

            public MapperConfig MapperConfig { get; set; } = new MapperConfig();
        }

        public SelectStatementBuilder(string schemaName) : base(schemaName)
        {
        }

        public SelectStatementBuilder<T> Join<TSelf, TRef>(params JoinParam[] mappings) where TRef : class, new()
        {
            JoinCfg ??= new JoinConfig();
            var config = GetConfig<TSelf, TRef, InnerJoinConfig>();

            config.JoinParams.AddRange(mappings);
            JoinCfg.MapperConfig.Include<TRef>();
            return this;
        }

        private TConfig GetConfig<TSelf, TRef, TConfig>() where TConfig : InnerJoinConfig, new()
        {
            if (JoinCfg!.JoinConfigs.TryGetValue((typeof(TSelf), typeof(TRef)), out var config)) return (TConfig)config;
            config = new TConfig();
            JoinCfg?.JoinConfigs.Add((typeof(TSelf), typeof(TRef)), config);

            return (TConfig)config;
        }

        public SelectStatementBuilder<T> LeftOuterJoin<TSelf, TRef>(IEnumerable<string> keys, params JoinParam[] mappings)
            where TRef : class, new()
        {
            JoinCfg ??= new JoinConfig();
            var config = GetConfig<TSelf, TRef, LeftOuterJoinConfig>();

            config.JoinParams.AddRange(mappings);
            config.NullKeys.AddRange(keys);
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
                            .Invoke(this, new object[] { list, config });
                    continue;
                }

                var alias = $"{typeof(TSelect).Name}_{propertyInfo.Name}";
                list.Add($"{tableName}.{propertyInfo.Name} as {alias}");
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
            foreach (var ((selfTable, refTable), joinConfig) in JoinCfg.JoinConfigs)
            {
                var selfTableName = WithSchema(selfTable.Name);
                var refTableName = WithSchema(refTable.Name);
                var constraints =
                    string.Join(
                        " and ",
                        joinConfig.JoinParams.Select(
                            jp => $"{selfTableName}.{jp.SelfColumn} = {refTableName}.{jp.ForeignColumn}"));

                string joinType;
                string constraintString;
                if (joinConfig is LeftOuterJoinConfig leftOuterJoinConfig)
                {
                    constraintString = string.Join(
                        " and ",
                        leftOuterJoinConfig.NullKeys.Select(
                            nk => $"{selfTableName}.{nk} is null"), constraints);
                    joinType = "left outer join";
                }
                else
                {
                    joinType = "join";
                    constraintString = constraints;
                }

                joins.Add($"{joinType} {refTableName} on {constraintString}");
            }

            return $" {string.Join(" ", joins)}";
        }
    }
}