using Hurace.Dal.Domain;

namespace Hurace.Core.Api
{
    public class TimeDifference
    {
        public TimeData TimeData { get; set; }
        public int DifferenceToLeader { get; set; }
    }
}