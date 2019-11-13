using Hurace.Core.Common;
using Hurace.Core.Common.QueryBuilder;
using Hurace.Core.Common.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dal.Dao.Base;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceDataDao : DefaultCrudDao<RaceData>, IRaceDataDao
    {
        public RaceDataDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.RaceData", statementFactory)
        {
        }

        private protected override SelectStatementBuilder<RaceData> DefaultSelectQuery() =>
            StatementFactory.Select<RaceData>().Join<RaceData, EventType>(("eventTypeId", "id"));
    }
}