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
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto.Util;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public abstract class BaseDao<T> : IBaseDao<T> where T : class, new()
    {
        private IConnectionFactory ConnectionFactory { get; }
        protected string TableName { get; }
        protected readonly QueryFactory _queryFactory;

        protected BaseDao(IConnectionFactory connectionFactory, string tableName, QueryFactory queryFactory)
        {
            ConnectionFactory = connectionFactory;
            TableName = tableName;
            _queryFactory = queryFactory;
        }

        protected async Task<int> ExecuteAsync(string statement, params QueryParam[] queryParams) =>
            await ConnectionFactory.UseConnection(statement, queryParams,
                                                  async command =>
                                                      await command.ExecuteNonQueryAsync());

        protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string statement,
            MapperConfig mapperConfig = null,
            params QueryParam[] queryParams) where TResult : class, new()
        {
            return await ConnectionFactory.UseConnection(statement, queryParams, async command =>
            {
                var items = new List<TResult>();
                await using var reader = await command.ExecuteReaderAsync();
                while (reader.Read()) items.Add(reader.MapTo<TResult>(mapperConfig));

                return items;
            });
        }

        protected async Task<bool>
            GeneratedExecutionAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            (await ExecuteAsync(data.statement, data.queryParams.ToArray())) == 1;

        protected async Task<IEnumerable<T>> GeneratedQueryAsync(
            (string statement, MapperConfig config, IEnumerable<QueryParam> queryParams) data) =>
            await QueryAsync<T>(data.statement, data.config, data.queryParams.ToArray());

        public abstract Task<bool> UpdateAsync(T obj);

        public async Task<bool> InsertAsync(T obj) =>
            await GeneratedExecutionAsync(_queryFactory.Insert<T>().Build(obj));

        public virtual async Task<IEnumerable<T>> FindAllAsync() =>
            await GeneratedQueryAsync(_queryFactory.Select<T>().Build());

        public abstract Task<T?> FindByIdAsync(int id);

        public virtual async Task<bool> DeleteAsync(int id) =>
            (await ExecuteAsync($"delete from {TableName} where id=@id", ("@id", id))) == 1;
    }
}