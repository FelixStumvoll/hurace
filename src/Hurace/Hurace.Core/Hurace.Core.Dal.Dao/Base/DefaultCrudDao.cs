using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.QueryBuilder;
using Hurace.Dal.Interface.Base;

namespace Hurace.Core.Dal.Dao.Base
{
    public class DefaultCrudDao<T> : CrudDao<T>, IDefaultCrudDao<T> where T : class, new()
    {
        protected DefaultCrudDao(IConnectionFactory connectionFactory, string tableName, StatementFactory statementFactory)
            : base(connectionFactory, tableName, statementFactory)
        {
        }

        public virtual async Task<T?> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(DefaultSelectQuery().Where<T>(("id", id)).Build())).SingleOrDefault();

        public virtual Task<int> InsertGetIdAsync(T obj) =>
            GeneratedNonQueryGetIdAsync(StatementFactory.Insert<T>().Build(obj));

        public virtual Task<bool> DeleteAsync(int id) =>
            ExecuteAsync($"delete from {TableName} where id=@id", ("@id", id));
    }
}