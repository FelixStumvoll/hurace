using Hurace.Core.Logic.Models;

namespace Hurace.API.Dtos.TimeDifferenceDtos
{
    public class TimeDifferenceDto
    {
        public int Time { get; set; }
        public int SensorNumber { get; set; }
        public double DifferenceToLeader { get; set; }

        public static TimeDifferenceDto FromTimeDifference(TimeDifference timeDifference) => new TimeDifferenceDto
        {
            Time = timeDifference.TimeData.Time,
            SensorNumber =  timeDifference.TimeData.Sensor.SensorNumber,
            DifferenceToLeader = timeDifference.DifferenceToLeader.TotalMilliseconds
        };
    }
}