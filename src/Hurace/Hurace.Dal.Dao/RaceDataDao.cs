using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
{
    public class RaceDataDao : DefaultCrudDao<RaceData>, IRaceDataDao
    {
        public RaceDataDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.RaceData", statementFactory)
        {
        }

        private protected override SelectStatementBuilder<RaceData> DefaultSelectQuery() =>
            StatementFactory.Select<RaceData>().Join<RaceData, EventType>((nameof(RaceData.EventTypeId), nameof(EventType.Id)));
    }
}