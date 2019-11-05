using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
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
                .Join<RaceEvent, RaceData>(("raceDataId", "id"))
                .Join<RaceData, EventType>(("eventTypeId", "id"));
    }
}