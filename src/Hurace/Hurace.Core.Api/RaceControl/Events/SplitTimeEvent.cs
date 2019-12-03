using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceControl.Events
{
    public class SplitTimeEvent : Event
    {
        public TimeData TimeData { get; set; }
    }
}