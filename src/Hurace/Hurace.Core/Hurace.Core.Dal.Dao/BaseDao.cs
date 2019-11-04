using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public abstract class BaseDao<T> where T : class, new()
    {
        protected IConnectionFactory ConnectionFactory { get; }
        protected string TableName { get; }
        protected readonly StatementFactory StatementFactory;

        protected BaseDao(IConnectionFactory connectionFactory, string tableName, StatementFactory statementFactory)
        {
            StatementFactory = statementFactory;
            TableName = tableName;
            ConnectionFactory = connectionFactory;
        }

        #region Select

        protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string statement,
            MapperConfig? mapperConfig = null,
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

        protected async Task<IEnumerable<T>> GeneratedQueryAsync(
            (string statement, MapperConfig config, IEnumerable<QueryParam> queryParams) data) =>
            await QueryAsync<T>(data.statement, data.config, data.queryParams.ToArray());

        protected virtual SelectStatementBuilder<T> DefaultSelectQuery() => StatementFactory.Select<T>();
        
        public virtual async Task<IEnumerable<T>> FindAllAsync() =>
            await GeneratedQueryAsync(DefaultSelectQuery().Build());

        public virtual async Task<T?> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(DefaultSelectQuery().Where<T>(("id", id)).Build())).SingleOrDefault();

        #endregion

        #region Update

        public virtual async Task<bool> UpdateAsync(T obj) =>
            await GeneratedExecutionAsync(StatementFactory
                                          .Update<T>()
                                          .WhereId(obj)
                                          .Build(obj));

        #endregion

        #region Delete

        public virtual async Task<bool> DeleteAsync(int id) =>
            await ExecuteAsync($"delete from {TableName} where id=@id", ("@id", id));

        public virtual async Task DeleteAllAsync() => await ExecuteAsync($"delete from {TableName}");

        #endregion

        #region Insert

        public virtual async Task<bool> InsertAsync(T obj) =>
            await GeneratedExecutionAsync(StatementFactory.Insert<T>().Build(obj));
        
        public virtual async Task<int> InsertGetIdAsync(T obj) =>
            await GeneratedExecutionGetIdAsync(StatementFactory.Insert<T>().Build(obj));

        #endregion
        
        protected async Task<int> ExecuteGetIdAsync(string statement, params QueryParam[] queryParams) =>
            await ConnectionFactory.UseConnection($"{statement};SELECT CAST(scope_identity() AS int)", 
                                                  queryParams,
                                                  async command =>
                                                      (int) await command.ExecuteScalarAsync());

        protected async Task<bool> ExecuteAsync(string statement, params QueryParam[] queryParams) =>
            (await ConnectionFactory.UseConnection(statement, queryParams,
                                                   async command =>
                                                       await command.ExecuteNonQueryAsync())) == 1;
        
        protected async Task<bool>
            GeneratedExecutionAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            await ExecuteAsync(data.statement, data.queryParams.ToArray());


        protected async Task<int>
            GeneratedExecutionGetIdAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            await ExecuteGetIdAsync(data.statement, data.queryParams.ToArray());
    }
}