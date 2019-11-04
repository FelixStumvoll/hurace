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
        private protected IConnectionFactory ConnectionFactory { get; }
        private protected string TableName { get; }
        private protected readonly StatementFactory StatementFactory;

        protected BaseDao(IConnectionFactory connectionFactory, string tableName, StatementFactory statementFactory)
        {
            StatementFactory = statementFactory;
            TableName = tableName;
            ConnectionFactory = connectionFactory;
        }

        #region Select

        private protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string statement,
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

        private protected async Task<IEnumerable<T>> GeneratedQueryAsync(
            (string statement, MapperConfig config, IEnumerable<QueryParam> queryParams) data) =>
            await QueryAsync<T>(data.statement, data.config, data.queryParams.ToArray());

        private protected virtual SelectStatementBuilder<T> DefaultSelectQuery() => StatementFactory.Select<T>();
        
        public virtual async Task<IEnumerable<T>> FindAllAsync() =>
            await GeneratedQueryAsync(DefaultSelectQuery().Build());

        private protected async Task<T?> DefaultFindById(int id) =>
            (await GeneratedQueryAsync(DefaultSelectQuery().Where<T>(("id", id)).Build())).SingleOrDefault();

        #endregion

        #region Update

        private protected async Task<bool> DefaultUpdate(T obj) =>
            await GeneratedExecutionAsync(StatementFactory
                                          .Update<T>()
                                          .WhereId(obj)
                                          .Build(obj));
        #endregion

        #region Delete

        private protected async Task<bool> DefaultDelete(int id) =>
            await ExecuteAsync($"delete from {TableName} where id=@id", ("@id", id));

        private protected async Task DefaultDeleteAll() => await ExecuteAsync($"delete from {TableName}");

        #endregion

        #region Insert

        private protected async Task<bool> DefaultInsert(T obj) =>
            await GeneratedExecutionAsync(StatementFactory.Insert<T>().Build(obj));
        
        private protected async Task<int> DefaultInsertGetId(T obj) =>
            await GeneratedExecutionGetIdAsync(StatementFactory.Insert<T>().Build(obj));

        #endregion
        
        private protected async Task<int> ExecuteGetIdAsync(string statement, params QueryParam[] queryParams) =>
            await ConnectionFactory.UseConnection($"{statement};SELECT CAST(scope_identity() AS int)", 
                                                  queryParams,
                                                  async command =>
                                                      (int) await command.ExecuteScalarAsync());

        private protected async Task<bool> ExecuteAsync(string statement, params QueryParam[] queryParams) =>
            (await ConnectionFactory.UseConnection(statement, queryParams,
                                                   async command =>
                                                       await command.ExecuteNonQueryAsync())) == 1;
        
        private protected async Task<bool>
            GeneratedExecutionAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            await ExecuteAsync(data.statement, data.queryParams.ToArray());


        private protected async Task<int>
            GeneratedExecutionGetIdAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            await ExecuteGetIdAsync(data.statement, data.queryParams.ToArray());
    }
}