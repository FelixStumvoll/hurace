using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Dal.Common
{
    public interface IConnectionFactory
    {
        Task<T> UseConnection<T>(string statement, IEnumerable<QueryParam> queryParams,
            Func<DbCommand, Task<T>> connectionFunc);
    }
}