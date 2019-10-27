using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto;
using Hurace.Core.Dto.Util;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public abstract class BaseDao<T> : IBaseDao<T> where T : class, new()
    {
        private IConnectionFactory ConnectionFactory { get; }
        protected string TableName { get; }

        protected BaseDao(IConnectionFactory connectionFactory, string tableName)
        {
            ConnectionFactory = connectionFactory;
            TableName = tableName;
        }

        protected async Task<int> ExecuteAsync(string statement, params QueryParam[] queryParams) =>
            await ConnectionFactory.UseConnection(statement, queryParams,
                                                  async command =>
                                                      await command.ExecuteNonQueryAsync());

        protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string statement,
            MapperConfig mapperConfig = null,
            params QueryParam[] queryParams) where TResult : new()
        {
            return await ConnectionFactory.UseConnection(statement, queryParams, async command =>
            {
                var items = new List<TResult>();
                await using var reader = await command.ExecuteReaderAsync();
                while (reader.Read()) items.Add(reader.MapTo<TResult>(mapperConfig));

                return items;
            });
        }

        public virtual Task<IEnumerable<T>> FindAllAsync() => QueryAsync<T>($"select * from {TableName}");

        public virtual Task<IEnumerable<T>> FindAllWhereAsync(string condition, params QueryParam[] queryParams) =>
            QueryAsync<T>($"select * from {TableName} where {condition}", queryParams: queryParams);

        public virtual async Task<T?> FindByIdAsync(int id) =>
            (await QueryAsync<T>($"select * from {TableName} where id=@id", queryParams:
                                 ("@id", id))).FirstOrDefault();


        private IEnumerable<(string name, object value)> GetMapProperties(object o) =>
            o.GetType()
                .GetProperties().Where(pi => pi.Name != "Id" && !Attribute.IsDefined(pi, typeof(NavigationalAttribute)))
                .Select(pi => (pi.Name, pi.GetValue(o)));

        private (string statement, IEnumerable<QueryParam> queryParams) GetUpdateData(T obj)
        {
            var strBuilder = new StringBuilder($"update {TableName} set ");
            var queryParams = new List<QueryParam>();
            var properties = GetMapProperties(obj).ToList();
            properties.Take(properties.Count-1).ToList().ForEach(data =>
            {
                strBuilder.Append($"{data.name}=@{data.name},");
                queryParams.Add(($"@{data.name}", data.value));
            });
            var (name, value) = properties.Last();
            strBuilder.Append($"{name}=@{name} where id=@id");
            
            queryParams.Add(($"@{name}", value));
            queryParams.Add(("@id", typeof(T).GetProperty("Id")?.GetValue(obj)));
            return (strBuilder.ToString(), queryParams);
        }

        private (string statement, IEnumerable<QueryParam> queryParams) GetInsertData(T obj)
        {
            var strBuilder = new StringBuilder($"insert into {TableName} (");
            var valueStrBuilder = new StringBuilder("values(");
            var queryParams = new List<QueryParam>();
            var properties = GetMapProperties(obj).ToList();
            properties.Take(properties.Count-1)
                .ToList()
                .ForEach(data =>
                {
                    strBuilder.Append($"{data.name},");
                    valueStrBuilder.Append($"@{data.name},");
                    queryParams.Add(($"@{data.name}", data.value));
                });
            var (name, value) = properties.Last();
            strBuilder.Append($"{name})");
            valueStrBuilder.Append($"@{value})");
            queryParams.Add(($"@{name}", value));

            return (strBuilder.Append(valueStrBuilder.ToString()).ToString(), queryParams);
        }

        public virtual async Task<bool> UpdateAsync(T obj)
        {
            var (statement, queryParams) = GetUpdateData(obj);
            return (await ExecuteAsync(statement, queryParams.ToArray()) == 1);
        }

        public virtual async Task<bool> DeleteAsync(int id) =>
            (await ExecuteAsync($"delete from {TableName} where id=@id", ("@id", id))) == 1;

        public virtual async Task<bool> InsertAsync(T obj)
        {
            var (statement, queryParams) = GetInsertData(obj);
            return (await ExecuteAsync(statement, queryParams.ToArray()) == 1);

        }
    }
}