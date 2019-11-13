using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.StatementBuilder;
using Hurace.Dal.Interface.Base;

namespace Hurace.Core.Dal.Dao.Base
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