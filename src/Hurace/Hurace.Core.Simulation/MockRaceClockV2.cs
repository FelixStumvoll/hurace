using System;
using Hurace.Core.Timer;
using MathNet.Numerics.Distributions;
using Microsoft.Extensions.Configuration;

namespace Hurace.Core.Simulation
{
    public class MockRaceClockV2 : IRaceClock
    {
        public event TimingTriggeredHandler TimingTriggered;

        public double Mean { get; set; }
        public double Deviation { get; set; }
        public int NextSensor { get; set; }
        public int SensorCount { get; set; }
        public bool Running { get; set; }
        public int MillisecondsLeft { get; set; }
        private readonly System.Timers.Timer _timer;

        public MockRaceClockV2()
        {
            var config = new ConfigurationBuilder().AddJsonFile("clocksettings.json").Build();
            var section = config.GetSection("ClockSettings");
            Mean = Convert.ToDouble(section["Mean"]);
            Deviation = Convert.ToDouble(section["Deviation"]);
            SensorCount = Convert.ToInt32(section["MaxSensorsOnStart"]);
            SetMillisLeft();
            _timer = new System.Timers.Timer(1);
            _timer.Elapsed += (sender, args) => OnTick();
        }

        private void SetMillisLeft() => MillisecondsLeft = (int) new Normal(Mean * 100, Deviation * 100).Sample();

        private void OnTick()
        {
            MillisecondsLeft -= 1;
            Console.WriteLine(MillisecondsLeft);
            if (MillisecondsLeft > 0 && NextSensor != 0) return;
            TimingTriggered?.Invoke(NextSensor, DateTime.Now);
            NextSensor++;
            SetMillisLeft();
            if (NextSensor >= SensorCount)
            {
                _timer.Stop();
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Reset()
        {
            NextSensor = 0;
        }

        public void SkipNextSensor()
        {
            NextSensor++;
        }

        public void TriggerSensor(int sensorNumber)
        {
            TimingTriggered?.Invoke(sensorNumber, DateTime.Now);
        }
    }
}