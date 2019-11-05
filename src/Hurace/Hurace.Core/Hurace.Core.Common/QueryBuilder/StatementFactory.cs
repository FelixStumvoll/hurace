using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;

namespace Hurace.Core.Dal.Dao.QueryBuilder
{
    public class StatementFactory
    {
        private readonly string _schemaName;

        public StatementFactory(string schemaName) => _schemaName = schemaName;

        public SelectStatementBuilder<T> Select<T>() where T : class, new() =>
            new SelectStatementBuilder<T>(_schemaName);

        public UpdateStatementBuilder<T> Update<T>() where T : class, new() => new UpdateStatementBuilder<T>(_schemaName);
        public InsertStatementBuilder<T> Insert<T>() => new InsertStatementBuilder<T>(_schemaName);
    }
}