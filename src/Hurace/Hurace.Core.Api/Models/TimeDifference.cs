using System;
using Hurace.Dal.Domain;

namespace Hurace.Core.Api.Models
{
    public class TimeDifference
    {
        public TimeData TimeData { get; set; }
        public TimeSpan DifferenceToLeader { get; set; }

        public TimeDifference(TimeData timeData, TimeSpan differenceToLeader)
        {
            TimeData = timeData;
            DifferenceToLeader = differenceToLeader;
        }
    }
}