using Hurace.Core.Interface.Entities;

namespace Hurace.API.Dtos.TimeDifferenceDtos
{
    public class TimeDifferenceDto
    {
        public int Time { get; set; }
        public int SensorNumber { get; set; }
        public double DifferenceToLeader { get; set; }

        private TimeDifferenceDto(int time, int sensorNumber, double differenceToLeader)
        {
            Time = time;
            SensorNumber = sensorNumber;
            DifferenceToLeader = differenceToLeader;
        }

        public static TimeDifferenceDto FromTimeDifference(TimeDifference timeDifference) => new TimeDifferenceDto
        (
            timeDifference.TimeData.Time,
            timeDifference.TimeData.Sensor?.SensorNumber ?? 0,
            timeDifference.DifferenceToLeader.TotalMilliseconds
        );
    }
}