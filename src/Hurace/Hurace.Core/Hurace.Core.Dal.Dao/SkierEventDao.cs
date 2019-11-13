using Hurace.Core.Common;
using Hurace.Core.Common.QueryBuilder;
using Hurace.Core.Common.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dal.Dao.Base;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SkierEventDao : DefaultCrudDao<SkierEvent>, ISkierEventDao
    {
        public SkierEventDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.SkierEvent", statementFactory)
        {
        }

        private protected override SelectStatementBuilder<SkierEvent> DefaultSelectQuery() =>
            StatementFactory.Select<SkierEvent>()
                            .Join<SkierEvent, RaceData>(("raceDataId", "id"))
                            .Join<RaceData, EventType>(("eventTypeId", "id"))
                            .Join<SkierEvent, StartList>(("raceId", "raceId"), ("skierId", "skierId"));
    }
}