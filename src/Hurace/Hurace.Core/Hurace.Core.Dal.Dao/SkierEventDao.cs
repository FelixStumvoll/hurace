using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder;
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