using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Domain.Interfaces;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Dao.Base
{
    public class DefaultReadonlyDao<T> : ReadonlyDao<T>, IDefaultReadonlyDao<T> where T : class, ISinglePkEntity, new()
    {
        protected DefaultReadonlyDao(IConnectionFactory connectionFactory, string tableName,
            StatementFactory statementFactory) : base(connectionFactory, tableName, statementFactory)
        {
        }

        public async Task<T?> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(DefaultSelectQuery().Where<T>((nameof(ISinglePkEntity.Id), id)).Build()))
            .SingleOrDefault();
    }
}