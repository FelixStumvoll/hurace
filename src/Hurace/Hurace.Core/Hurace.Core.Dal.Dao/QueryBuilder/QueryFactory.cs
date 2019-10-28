using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;

namespace Hurace.Core.Dal.Dao.QueryBuilder
{
    public class QueryFactory
    {
        private readonly string _schemaName;

        public QueryFactory(string schemaName) => _schemaName = schemaName;

        public SelectQueryBuilder<T> Select<T>() where T : class, new() =>
            new SelectQueryBuilder<T>(_schemaName);

        public UpdateQueryBuilder<T> Update<T>() => new UpdateQueryBuilder<T>(_schemaName);
        public InsertQueryBuilder<T> Insert<T>() => new InsertQueryBuilder<T>(_schemaName);
    }
}