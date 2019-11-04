using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Dal.Interface.Util;

namespace Hurace.Core.Dal.Dao
{
    public class CrudDao<T> : ReadonlyDao<T>, ICrudDao<T> where T : class, new()
    {
        public CrudDao(IConnectionFactory connectionFactory, string tableName, StatementFactory statementFactory) :
            base(connectionFactory, tableName, statementFactory)
        {
        }

        public virtual Task<bool> UpdateAsync(T obj) => GeneratedNonQueryAsync(StatementFactory
                                                                        .Update<T>()
                                                                        .WhereId(obj)
                                                                        .Build(obj));
        public virtual Task<bool> InsertAsync(T obj) =>
            GeneratedNonQueryAsync(StatementFactory.Insert<T>().Build(obj));

        public virtual Task DeleteAllAsync() => ExecuteAsync($"delete from {TableName}");
    }
}