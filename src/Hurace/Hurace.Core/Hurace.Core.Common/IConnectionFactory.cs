using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Core.Common
{
    public interface IConnectionFactory
    {
        string ConnectionString { get; set; }
        string ProviderName { get; set; }

        Task<T> UseConnection<T>(string statement, IEnumerable<QueryParam> queryParams,
            Func<DbCommand, Task<T>> connectionFunc);
    }
}