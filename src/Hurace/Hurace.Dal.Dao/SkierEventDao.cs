using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
{
    public class SkierEventDao : DefaultCrudDao<SkierEvent>, ISkierEventDao
    {
        public SkierEventDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.SkierEvent", statementFactory)
        {
        }

        private protected override SelectStatementBuilder<SkierEvent> DefaultSelectQuery() =>
            StatementFactory.Select<SkierEvent>()
                            .Join<SkierEvent, RaceData>((nameof(SkierEvent.RaceDataId), nameof(RaceData.Id)))
                            .Join<RaceData, EventType>((nameof(RaceData.EventTypeId), nameof(EventType.Id)))
                            .Join<SkierEvent, StartList>((nameof(SkierEvent.RaceId), nameof(StartList.RaceId)),
                                                         (nameof(SkierEvent.SkierId), nameof(StartList.SkierId)));
    }
}