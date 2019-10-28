using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Hurace.Core.Common;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dal.Dao.QueryBuilder
{
    public class QueryBuilder
    {
        private string _schemaName;

        private QueryBuilder(string schemaName)
        {
            _schemaName = schemaName;
        }

        private string WithSchema(string append) => $"{_schemaName}.{append}";

        private static IEnumerable<(string name, object value)> GetCrudProperties(object obj) =>
            obj.GetType()
                .GetProperties()
                .Where(pi => pi.Name != "Id" && !Attribute.IsDefined(pi, typeof(NavigationalAttribute)))
                .Select(pi => (pi.Name, pi.GetValue(obj)));

        public class SelectQueryBuilder<T> : QueryBuilder where T : class, new()
        {
            private readonly Dictionary<(Type, Type), List<JoinParam>> _joinMappings = new
                Dictionary<(Type, Type), List<JoinParam>>();

            private readonly HashSet<Type> _inclusion = new HashSet<Type>();

            public SelectQueryBuilder<T> Join<TSelf, TRef>(params JoinParam[] mappings)
            {
                if (!_joinMappings.TryGetValue((typeof(TSelf), typeof(TRef)), out var list))
                {
                    list = new List<JoinParam>();
                    _joinMappings.Add((typeof(TSelf), typeof(TRef)), list);
                }

                list.AddRange(mappings);
                _inclusion.Add(typeof(TRef));
                return this;
            }

            private void AppendColumns<TSelect>(StringBuilder builder, MapperConfig config) where TSelect : class, new()
            {
                var tableName = $"hurace.{typeof(TSelect).Name}";
                foreach (var propertyInfo in typeof(TSelect).GetProperties())
                {
                    if (Attribute.IsDefined(propertyInfo, typeof(NavigationalAttribute)))
                    {
                        if (_inclusion.Contains(propertyInfo.PropertyType))
                            typeof(SelectQueryBuilder<T>)
                                .GetMethod(nameof(AppendColumns), BindingFlags.NonPublic | BindingFlags.Instance)
                                ?.MakeGenericMethod(propertyInfo.PropertyType)
                                .Invoke(this, new object[] {builder, config});
                        continue;
                    }

                    var alias = $"{typeof(TSelect).Name}_{propertyInfo.Name}";
                    builder.Append($"{tableName}.{propertyInfo.Name.ToLowerFirstChar()} as {alias},");
                    config.AddMapping<TSelect>((alias, propertyInfo.Name));
                }
            }


            private void HandleColumns(StringBuilder strBuilder, MapperConfig config)
            {
                var tableName = WithSchema(typeof(T).Name);
                AppendColumns<T>(strBuilder, config);
                if (strBuilder.Length > 0) strBuilder.Remove(strBuilder.Length - 1, 1); //removes last ,
                strBuilder.Append($" from {tableName}");
            }

            private void HandleJoins(StringBuilder strBuilder)
            {
                foreach (var ((selfTable, refTable), joinConstraints) in _joinMappings)
                {
                    var selfTableName = WithSchema(selfTable.Name);
                    var refTableName = WithSchema(refTable.Name);
                    strBuilder.Append($" join {refTableName} on");
                    joinConstraints.ForEach(cm => strBuilder.Append(
                                                $" {selfTableName}.{cm.SelfColumn} = {refTableName}.{cm.ForeignColumn} and"));
                    if (joinConstraints.Count > 0) strBuilder.Remove(strBuilder.Length - 3, 3);
                }
            }

            public (string, MapperConfig) Build()
            {
                var config = new MapperConfig();
                var strBuilder = new StringBuilder("Select ");
                HandleColumns(strBuilder, config);
                HandleJoins(strBuilder);


                return (strBuilder.ToString(), config);
            }

            public SelectQueryBuilder(string schemaName) : base(schemaName)
            {
            }
        }

        public class UpdateQueryBuilder<T> : QueryBuilder
        {
            private readonly List<QueryParam> _whereProperties = new List<QueryParam>();

            public UpdateQueryBuilder(string schemaName) : base(schemaName)
            {
            }

            public UpdateQueryBuilder<T> Where(params QueryParam[] whereProperties)
            {
                _whereProperties.AddRange(whereProperties);
                return this;
            }

            public (string, IEnumerable<QueryParam>) Build(T obj)
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

                queryParams.AddRange(_whereProperties.Select(queryParam => new QueryParam
                {
                    Name = $"@{queryParam.Name}", Value = queryParam.Value
                }));

                var whereSection =
                    string.Join(
                        "and",
                        _whereProperties
                            .Select(prop => $"{WithSchema(typeof(T).Name)}.{prop.Name}=@{prop.Name}"));
                return ($"update {WithSchema(typeof(T).Name)} set {string.Join(',', updateProps)} where" +
                        $" {whereSection}", queryParams);
            }
        }

        public class InsertQueryBuilder<T> : QueryBuilder
        {
            public InsertQueryBuilder(string schemaName) : base(schemaName)
            {
            }

            public (string, IEnumerable<QueryParam>) Build(T obj)
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

        public static SelectQueryBuilder<T> SelectAll<T>(string schemaName) where T : class, new() =>
            new SelectQueryBuilder<T>(schemaName);

        public static UpdateQueryBuilder<T> Update<T>(string schemaName) => new UpdateQueryBuilder<T>(schemaName);
        public static InsertQueryBuilder<T> Insert<T>(string schemaName) => new InsertQueryBuilder<T>(schemaName);
    }
}