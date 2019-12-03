namespace Hurace.Core.Api.RaceControl.Events
{
    public class RaceEvent : Event
    {
        public Dal.Domain.Race Race { get; set; }
    }
}