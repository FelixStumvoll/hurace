using System;
using System.Threading;
using Hurace.Core.Timer;
using MathNet.Numerics.Distributions;

namespace Hurace.Core.Simulation
{
    public class MockRaceClock : IRaceClock
    {
        public event TimingTriggeredHandler TimingTriggered;

        public static MockRaceClock Instance { get; private set; }

        public int Average
        {
            get
            {
                lock (_lockObj) return _average;
            }
            set
            {
                lock (_lockObj) _average = value;
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

        public int Deviation
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

        public bool SkipNext
        {
            get
            {
                lock (_lockObj) return _skipNext;
            }
            set
            {
                lock (_lockObj) _skipNext = value;
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
        private readonly ManualResetEvent mrse = new ManualResetEvent(false);
        private bool _terminated;
        private int _deviation = 500;
        private int _average = 20000;
        private bool _running;
        private int _currentSensor;
        private int _maxSensor = 5;
        private bool _skipNext;
        private int _sensorCount;

        public MockRaceClock()
        {
            var clockThread = new Thread(() =>
            {
                while (!Terminated)
                {
                    mrse.WaitOne();
                    if (!SkipNext)
                    {
                        TimingTriggered?.Invoke(CurrentSensor, DateTime.Now);
                        SkipNext = false;
                       
                        CurrentSensor++;
                        if (CurrentSensor > MaxSensor)
                        {
                            CurrentSensor = 0;
                            mrse.Reset();
                        }
                    }

                    var normalDist = new Normal(Average, Deviation);
                    var time = normalDist.Sample();

                    Thread.Sleep((int) time);
                }
            });

            TimingTriggered += (id, time) => Console.WriteLine($"{id}: {time}");

            clockThread.Start();
            Instance = this;
        }

        public void Start()
        {
            mrse.Set();
            Running = true;
        }

        public void Stop()
        {
            mrse.Reset();
            Running = false;
        }

        public void TriggerSensor(int sensorId)
        {
            TimingTriggered?.Invoke(sensorId, DateTime.Now);
        }
    }
}