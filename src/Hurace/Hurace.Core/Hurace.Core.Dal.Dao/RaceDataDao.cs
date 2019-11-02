using System;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceDataDao : BaseDao<RaceData>, IRaceDataDao
    {
        public RaceDataDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.RaceData", statementFactory)
        {
        }

        protected override SelectStatementBuilder<RaceData> DefaultSelectQuery() =>
            StatementFactory.Select<RaceData>().Join<RaceData, EventType>(("eventTypeId", "id"));
    }
}