using System;
using System.Threading.Tasks;
using Hurace.Core.Timer;
using MathNet.Numerics.Distributions;
using Microsoft.Extensions.Configuration;

namespace Hurace.Core.Simulation
{
    public class MockRaceClock : IRaceClock
    {
        public event TimingTriggeredHandler TimingTriggered;
        public event Action<int> OnNextSensorChanged;
        public event Action<int> OnCountdownChanged;
        public event Action<bool> OnRunningChanged;

        private readonly System.Timers.Timer _timer;
        private int _millisecondsLeft;
        private int _nextSensor;
        private readonly object _lockObj = new object();
        private double _deviation;
        private double _mean;
        private int _sensorCount;

        public double Mean
        {
            get
            {
                lock (_lockObj) return _mean;
            }
            set
            {
                lock (_lockObj) _mean = value;
            }
        }

        public double Deviation
        {
            get
            {
                lock (_lockObj) return _deviation;
            }
            set
            {
                lock (_lockObj) _deviation = value;
            }
        }

        private int NextSensor
        {
            get
            {
                lock (_lockObj) return _nextSensor;
            }
            set
            {
                lock (_lockObj) _nextSensor = value;
            }
        }

        public int SensorCount
        {
            get
            {
                lock (_lockObj) return _sensorCount;
            }
            set
            {
                lock (_lockObj) _sensorCount = value;
            }
        }

        private int MillisecondsLeft
        {
            get
            {
                lock (_lockObj) return _millisecondsLeft;
            }
            set
            {
                lock (_lockObj) _millisecondsLeft = value;
            }
        }


        public MockRaceClock()
        {
            var config = new ConfigurationBuilder().AddJsonFile("clocksettings.json").Build();
            var section = config.GetSection("ClockSettings");
            Mean = Convert.ToDouble(section["Mean"]);
            Deviation = Convert.ToDouble(section["Deviation"]);
            SensorCount = Convert.ToInt32(section["MaxSensorsOnStart"]);
            _timer = new System.Timers.Timer(1);
            _timer.Elapsed += (sender, args) => OnTick();
            NextSensor = 0;
            MillisecondsLeft = 0;
        }

        private int GetNextWaitTime() => (int)new Normal(Mean * 100, Deviation * 100).Sample();

        private void OnTick()
        {
            lock (this)
            {
                MillisecondsLeft -= 1;
                OnCountdownChanged?.Invoke(MillisecondsLeft);
                if (MillisecondsLeft > 0) return;

                var sensor = NextSensor;
                TimingTriggered?.Invoke(sensor, DateTime.Now);

                if (NextSensor + 1 >= SensorCount)
                {
                    Reset();
                    return;
                }

                NextSensor++;
                OnNextSensorChanged?.Invoke(NextSensor);
                MillisecondsLeft = GetNextWaitTime();
            }
        }

        public void Start()
        {
            OnRunningChanged?.Invoke(true);
            if (NextSensor == 0)
            {
                MillisecondsLeft = 0;
            }

            _timer.Start();
        }

        public void Stop()
        {
            OnRunningChanged?.Invoke(false);
            _timer.Stop();
        }

        public void Reset()
        {
            _timer.Stop();
            MillisecondsLeft = 0;
            NextSensor = 0;
            OnRunningChanged?.Invoke(false);
            OnCountdownChanged?.Invoke(MillisecondsLeft);
            OnNextSensorChanged?.Invoke(NextSensor);
        }

        public void SkipNextSensor()
        {
            if (NextSensor + 1 >= SensorCount) return;
            NextSensor++;
            MillisecondsLeft += GetNextWaitTime();
            OnCountdownChanged?.Invoke(MillisecondsLeft);
            OnNextSensorChanged?.Invoke(NextSensor);
        }

        public void TriggerSensor(int sensorNumber)
        {
            TimingTriggered?.Invoke(sensorNumber, DateTime.Now);
        }
    }
}