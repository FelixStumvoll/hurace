using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Dal.Interface.Util;

namespace Hurace.Core.Dal.Dao
{
    public class DefaultBaseDao<T> : BaseDao<T>, IBaseDao<T>, ISingleIdBaseDao<T> where T : class, new()
    {
        protected DefaultBaseDao(IConnectionFactory connectionFactory, string tableName, StatementFactory statementFactory)
            : base(connectionFactory, tableName, statementFactory)
        {
            
        }

        public Task<bool> UpdateAsync(T obj) => DefaultUpdate(obj);

        public Task<bool> InsertAsync(T obj) => DefaultInsert(obj);

        public Task DeleteAllAsync() => DefaultDeleteAll();
        
        public Task<T> FindByIdAsync(int id) => DefaultFindById(id);
        public Task<int> InsertGetIdAsync(T obj) => DefaultInsertGetId(obj);

        public Task<bool> DeleteAsync(int id) => DefaultDelete(id);
    }
}