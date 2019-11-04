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
    public class RaceEventDao : BaseDao<RaceEvent>, IRaceEventDao
    {
        public RaceEventDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.RaceEvent", statementFactory)
        {
        }

        private protected override SelectStatementBuilder<RaceEvent> DefaultSelectQuery() =>
            StatementFactory
                .Select<RaceEvent>()
                .Join<RaceData, EventType>(("eventTypeId", "id"))
                .Join<RaceEvent, RaceData>(("raceDataId", "id"));

        public override async Task<bool> InsertAsync(RaceEvent obj) =>
            await GeneratedExecutionAsync(StatementFactory.Insert<RaceEvent>().WithKey().Build(obj));
        
        public override async Task<int> InsertGetIdAsync(RaceEvent obj) =>
            await GeneratedExecutionGetIdAsync(StatementFactory.Insert<RaceEvent>().WithKey().Build(obj));
    }
}