using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder;
using Hurace.Dal.Interface.Base;

namespace Hurace.Dal.Dao.Base
{
    public class ReadonlyDao<T> : BaseDao<T>, IReadonlyDao<T> where T : class, new()
    {
        protected ReadonlyDao(IConnectionFactory connectionFactory, string tableName,
            StatementFactory statementFactory) : base(connectionFactory, tableName, statementFactory)
        {
        }

        private protected virtual SelectStatementBuilder<T> DefaultSelectQuery() => StatementFactory.Select<T>();
        
        public virtual async Task<IEnumerable<T>> FindAllAsync() =>
            await GeneratedQueryAsync(DefaultSelectQuery().Build());
    }
}