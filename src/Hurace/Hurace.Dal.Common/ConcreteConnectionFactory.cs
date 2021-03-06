using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Hurace.Dal.Common.Extensions;

namespace Hurace.Dal.Common
{
    public class ConcreteConnectionFactory : IConnectionFactory
    {
        public string ConnectionString { get; }

        private readonly DbProviderFactory _dbProviderFactory;
        
        public ConcreteConnectionFactory(DbProviderFactory dbProviderFactory, string connectionString)
        {
            _dbProviderFactory = dbProviderFactory;
            ConnectionString = connectionString;
        }
        
        public async Task<T> UseConnection<T>(string statement, IEnumerable<QueryParam> queryParams,
            Func<DbCommand, Task<T>> connectionFunc)
        {
            await using var connection = _dbProviderFactory.CreateConnection();
            if (connection == null) throw new NullReferenceException("Could not create connection");
            connection.ConnectionString = ConnectionString;
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = statement;
            command.AddParameters(queryParams);
            return await connectionFunc(command);
        }
    }
}