using Hurace.Core.Common;
using Hurace.Core.Common.StatementBuilder;
using Hurace.Core.Dal.Dao.Base;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class EventTypeDao : DefaultReadonlyDao<EventType>, IEventTypeDao
    {
        protected EventTypeDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(connectionFactory, "hurace.eventType", statementFactory)
        {
        }
    }
}