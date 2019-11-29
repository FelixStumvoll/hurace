namespace Hurace.Dal.Domain
{
    public static class Constants
    {
        public enum RaceEvent
        {
            Started = 1,
            Finished = 2,
            Canceled = 3
        }

        public enum SkierEvent
        {
            Started = 4,
            Finished = 5,
            Disqualified = 6,
            Failure = 7,
            SplitTime = 8,
            Canceled = 9
        }

        public enum StartState
        {
            Upcoming = 1,
            Running = 2,
            Finished = 3,
            Disqualified = 4,
            Canceled = 5, 
            DrawReady = 6
        }

        public enum Gender
        {
            Male = 1,
            Female = 2
        }

        public enum RaceState
        {
            Upcoming = 1, 
            Running = 2,
            Finished = 3,
            Canceled = 4
        }
    }
}