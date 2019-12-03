using Hurace.Dal.Domain;

namespace Hurace.Core.Api.RaceControl.Events
{
    public class SkierEvent : Event
    {
        public Skier Skier { get; set; }
    }
}