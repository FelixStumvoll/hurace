using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public abstract class BaseDao<T> : ReadonlyBaseDao<T>, IBaseDao<T> where T : class, new()
    {
        protected BaseDao(IConnectionFactory connectionFactory, string tableName, StatementFactory statementFactory) : base(
            statementFactory, tableName, connectionFactory)
        {
        }

        protected async Task<int> ExecuteGetIdAsync(string statement, params QueryParam[] queryParams)
        {
            statement = $"{statement};SELECT CAST(scope_identity() AS int)";
            return await ConnectionFactory.UseConnection(statement, queryParams,
                                                         async command =>
                                                             (int) await command.ExecuteScalarAsync());
        }

        protected async Task<bool> ExecuteAsync(string statement, params QueryParam[] queryParams) =>
            (await ConnectionFactory.UseConnection(statement, queryParams,
                                                   async command =>
                                                       await command.ExecuteNonQueryAsync())) ==1;


        protected async Task<bool>
            GeneratedExecutionAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            await ExecuteAsync(data.statement, data.queryParams.ToArray());


        protected async Task<int>
            GeneratedExecutionWithIdAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            await ExecuteGetIdAsync(data.statement, data.queryParams.ToArray());

        public virtual async Task<bool> UpdateAsync(T obj) =>
            await GeneratedExecutionAsync(StatementFactory
                                          .Update<T>()
                                          .WhereId(obj)
                                          .Build(obj));

        public virtual async Task<bool> InsertAsync(T obj) =>
            await GeneratedExecutionAsync(StatementFactory.Insert<T>().Build(obj));


        public virtual async Task<int> InsertGetIdAsync(T obj) =>
            await GeneratedExecutionWithIdAsync(StatementFactory.Insert<T>().Build(obj));
        
        public virtual async Task DeleteAllAsync() => await ExecuteAsync($"delete from {TableName}");
    }
}