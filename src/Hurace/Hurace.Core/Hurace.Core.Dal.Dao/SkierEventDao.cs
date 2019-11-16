using Hurace.Core.Common;
using Hurace.Core.Common.StatementBuilder;
using Hurace.Core.Common.StatementBuilder.ConcreteStatementBuilder;
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
                            .Join<SkierEvent, RaceData>((nameof(SkierEvent.RaceDataId), nameof(RaceData.Id)))
                            .Join<RaceData, EventType>((nameof(RaceData.EventTypeId), nameof(EventType.Id)))
                            .Join<SkierEvent, StartList>((nameof(SkierEvent.RaceId), nameof(StartList.RaceId)),
                                                         (nameof(SkierEvent.SkierId), nameof(StartList.SkierId)));
    }
}