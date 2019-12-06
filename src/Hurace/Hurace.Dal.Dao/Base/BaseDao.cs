using Hurace.Dal.Common;
using Hurace.Dal.Common.Extensions;
using Hurace.Dal.Common.Mapper;
using Hurace.Dal.Common.StatementBuilder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Dal.Dao.Base
{
    public abstract class BaseDao<T> where T : class, new()
    {
        private IConnectionFactory ConnectionFactory { get; }
        private protected string TableName { get; }
        private protected readonly StatementFactory StatementFactory;

        protected BaseDao(IConnectionFactory connectionFactory, string tableName, StatementFactory statementFactory)
        {
            StatementFactory = statementFactory;
            TableName = tableName;
            ConnectionFactory = connectionFactory;
        }

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

        private protected async Task<int> ExecuteScalarAsync(string statement, params QueryParam[] queryParams) =>
            await ConnectionFactory.UseConnection(statement,
                                                  queryParams,
                                                  async command =>
                                                      (int) await command.ExecuteScalarAsync());

        private async Task<int> ExecuteGetIdAsync(string statement, params QueryParam[] queryParams) =>
            await ExecuteScalarAsync($"{statement};SELECT CAST(scope_identity() AS int)",
                                     queryParams);

        private protected async Task<bool> ExecuteAsync(string statement, params QueryParam[] queryParams) =>
            (await ConnectionFactory.UseConnection(statement, queryParams,
                                                   async command =>
                                                       await command.ExecuteNonQueryAsync())) == 1;

        private protected async Task<bool>
            GeneratedNonQueryAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            await ExecuteAsync(data.statement, data.queryParams.ToArray());


        private protected async Task<int>
            GeneratedNonQueryGetIdAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            await ExecuteGetIdAsync(data.statement, data.queryParams.ToArray());
    }
}