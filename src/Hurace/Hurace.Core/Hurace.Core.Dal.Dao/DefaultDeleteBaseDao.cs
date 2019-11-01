using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public abstract class DefaultDeleteBaseDao<T> : BaseDao<T>, IDefaultDeleteBaseDao<T> where T : class, new()
    {
        protected DefaultDeleteBaseDao(IConnectionFactory connectionFactory, string tableName,
            StatementFactory statementFactory) :
            base(connectionFactory, tableName, statementFactory)
        {
        }

        public async Task<bool> DeleteAsync(int id) =>
            await ExecuteAsync($"delete from {TableName} where id=@id", ("@id", id));
    }
}