using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
{
    public class RaceEventDao : DefaultCrudDao<RaceEvent>, IRaceEventDao
    {
        public RaceEventDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.RaceEvent", statementFactory)
        {
        }

        private protected override SelectStatementBuilder<RaceEvent> DefaultSelectQuery() =>
            StatementFactory
                .Select<RaceEvent>()
                .Join<RaceEvent, RaceData>((nameof(RaceEvent.RaceDataId),nameof(RaceData.Id)))
                .Join<RaceData, EventType>((nameof(RaceData.EventTypeId), nameof(EventType.Id)));
    }
}