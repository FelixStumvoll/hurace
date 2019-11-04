using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Dal.Interface;
using Hurace.Dal.Interface.Util;

namespace Hurace.Core.Dal.Dao
{
    public class ReadonlyDao<T> : BaseDao<T>, IReadonlyDao<T> where T : class, new()
    {
        public ReadonlyDao(IConnectionFactory connectionFactory, string tableName,
            StatementFactory statementFactory) : base(connectionFactory, tableName, statementFactory)
        {
        }

        private protected virtual SelectStatementBuilder<T> DefaultSelectQuery() => StatementFactory.Select<T>();
        
        public virtual async Task<IEnumerable<T>> FindAllAsync() =>
            await GeneratedQueryAsync(DefaultSelectQuery().Build());
    }
}