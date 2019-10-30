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
        protected BaseDao(IConnectionFactory connectionFactory, string tableName,QueryFactory queryFactory) : base(
            queryFactory, tableName, connectionFactory)
        {
        }

        protected async Task<int> ExecuteAsync(string statement, params QueryParam[] queryParams) =>
            await ConnectionFactory.UseConnection(statement, queryParams,
                                                  async command =>
                                                      await command.ExecuteNonQueryAsync());


        protected async Task<bool>
            GeneratedExecutionAsync((string statement, IEnumerable<QueryParam> queryParams) data) =>
            (await ExecuteAsync(data.statement, data.queryParams.ToArray())) == 1;

        

        public abstract Task<bool> UpdateAsync(T obj);

        public virtual async Task<bool> InsertAsync(T obj) =>
            await GeneratedExecutionAsync(QueryFactory.Insert<T>().Build(obj, "Id"));


        public virtual async Task<bool> DeleteAsync(int id) =>
            (await ExecuteAsync($"delete from {TableName} where id=@id", ("@id", id))) == 1;

        public async Task DeleteAllAsync() => await ExecuteAsync($"delete from {TableName}");
    }
}