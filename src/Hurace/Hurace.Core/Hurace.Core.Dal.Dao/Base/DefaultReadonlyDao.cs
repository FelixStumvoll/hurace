using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.QueryBuilder;
using Hurace.Dal.Interface.Base;

namespace Hurace.Core.Dal.Dao.Base
{
    public class DefaultReadonlyDao<T> : ReadonlyDao<T>, IDefaultReadonlyDao<T> where T : class, new()
    {
        protected DefaultReadonlyDao(IConnectionFactory connectionFactory, string tableName,
            StatementFactory statementFactory) : base(connectionFactory, tableName, statementFactory)
        {
        }

        public async Task<T?> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(DefaultSelectQuery().Where<T>(("id", id)).Build())).SingleOrDefault();
    }
}