using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public abstract class ReadonlyBaseDao<T> : IReadonlyBaseDao<T> where T : class, new()
    {
        protected IConnectionFactory ConnectionFactory { get; }
        protected string TableName { get; }
        protected  readonly QueryFactory QueryFactory;

        protected ReadonlyBaseDao(QueryFactory queryFactory, string tableName, IConnectionFactory connectionFactory)
        {
            QueryFactory = queryFactory;
            TableName = tableName;
            ConnectionFactory = connectionFactory;
        }

        protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string statement,
            MapperConfig? mapperConfig = null,
            params QueryParam[] queryParams) where TResult : class, new()
        {
            return await ConnectionFactory.UseConnection(statement, queryParams, async command =>
            {
                var items = new List<TResult>();
                await using var reader = await command.ExecuteReaderAsync();
                while (reader.Read()) items.Add(reader.MapTo<TResult>(mapperConfig));

                return items;
            });
        }
        
        protected async Task<IEnumerable<T>> GeneratedQueryAsync(
            (string statement, MapperConfig config, IEnumerable<QueryParam> queryParams) data) =>
            await QueryAsync<T>(data.statement, data.config, data.queryParams.ToArray());
        
        public virtual async Task<IEnumerable<T>> FindAllAsync() =>
            await GeneratedQueryAsync(QueryFactory.Select<T>().Build());
        
        public virtual async Task<T?> FindByIdAsync(int id) =>
            (await GeneratedQueryAsync(QueryFactory.Select<T>().Where<T>(("id", id)).Build())).SingleOrDefault();
    }
}