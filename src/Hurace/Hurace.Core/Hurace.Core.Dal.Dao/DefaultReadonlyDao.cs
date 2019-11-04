using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Dal.Interface.Util;

namespace Hurace.Core.Dal.Dao
{
    public class DefaultReadonlyDao<T> : ReadonlyDao<T>, IDefaultReadonlyDao<T> where T : class, new()
    {
        public DefaultReadonlyDao(IConnectionFactory connectionFactory, string tableName,
            StatementFactory statementFactory) : base(connectionFactory, tableName, statementFactory)
        {
        }

        public async Task<T> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(DefaultSelectQuery().Where<T>(("id", id)).Build())).SingleOrDefault();
    }
}