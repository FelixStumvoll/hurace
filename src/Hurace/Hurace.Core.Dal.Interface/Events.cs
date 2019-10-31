namespace Hurace.Core.Common
{
    public static class Events
    {
        public enum RaceEvent{Started = 1, Finished = 2, Canceled = 3}
        public enum SkierEvent{Started = 4, Finished = 5, Disqualified = 6, Failure = 7}
    }
}