using System;
using System.Threading;
using Hurace.Core.Timer;
using MathNet.Numerics.Distributions;
using Microsoft.Extensions.Configuration;

namespace Hurace.Core.Simulation
{
    public class MockRaceClock : IRaceClock
    {
        public event TimingTriggeredHandler TimingTriggered;

        public static MockRaceClock Instance { get; private set; }

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

        public bool Running
        {
            get
            {
                lock (_lockObj) return _running;
            }
            private set
            {
                lock (_lockObj) _running = value;
            }
        }

        public int CurrentSensor
        {
            get
            {
                lock (_lockObj) return _currentSensor;
            }
            set
            {
                lock (_lockObj) _currentSensor = value;
            }
        }

        public int MaxSensor
        {
            get
            {
                lock (_lockObj) return _maxSensor;
            }
            set
            {
                lock (_lockObj) _maxSensor = value;
            }
        }

        public bool Terminated
        {
            get
            {
                lock (_lockObj) return _terminated;
            }
            set
            {
                lock (_lockObj) _terminated = value;
            }
        }

        private readonly object _lockObj = new object();
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private bool _terminated;
        private double _deviation;
        private double _mean = 20000;
        private bool _running;
        private int _currentSensor;
        private int _maxSensor = 5;

        public MockRaceClock()
        {
            ConfigureClock();
            new Thread(ClockFunc).Start();
            Instance = this;
        }

        private void ClockFunc()
        {
            while (!Terminated)
            {
                _resetEvent.WaitOne();
                if (CurrentSensor <= MaxSensor) TimingTriggered?.Invoke(CurrentSensor, DateTime.Now);
                CurrentSensor++;

                if (CurrentSensor > MaxSensor)
                {
                    CurrentSensor = 0;
                    _resetEvent.Reset();
                }
                else
                {
                    var normalDist = new Normal(Mean * 1000, Deviation * 1000);
                    var time = normalDist.Sample();
                    Thread.Sleep((int) time);
                }
            }
        }

        private void ConfigureClock()
        {
            var config = new ConfigurationBuilder().AddJsonFile("clocksettings.json").Build();
            var section = config.GetSection("ClockSettings");
            Mean = Convert.ToDouble(section["Mean"]);
            Deviation = Convert.ToDouble(section["Deviation"]);
            MaxSensor = Convert.ToInt32(section["MaxSensorsOnStart"]);
        }

        public void Start()
        {
            _resetEvent.Set();
            Running = true;
        }

        public void Stop()
        {
            _resetEvent.Reset();
            Running = false;
        }

        public void TriggerSensor(int sensorId)
        {
            TimingTriggered?.Invoke(sensorId, DateTime.Now);
        }

        public void Reset()
        {
            CurrentSensor = 0;
        }

        public void SkipNext()
        {
            CurrentSensor++;
        }
    }
}