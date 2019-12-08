﻿using System;
using System.Threading;
using Hurace.Core.Timer;
using MathNet.Numerics.Distributions;

namespace Hurace.Core.Simulation
{
    public class MockRaceClock : IRaceClock
    {
        public event TimingTriggeredHandler TimingTriggered;

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
            set
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

        private DateTime _clockTime = DateTime.Now.Date;
        private DateTime _startTime;
        private object _lockObj = new object();
        private Thread _clockThread;
        private ManualResetEvent mrse = new ManualResetEvent(false);
        private bool _terminated;
        private int _deviation;
        private int _average;
        private bool _running;
        private int _currentSensor;
        private int _maxSensor;
        private bool _skipNext;

        public MockRaceClock()
        {
            _clockThread = new Thread(() =>
            {
                _startTime = _clockTime;
                while (!Terminated)
                {
                    mrse.WaitOne();
                    if (SkipNext)
                    {
                        TimingTriggered?.Invoke(CurrentSensor, _clockTime);
                        SkipNext = false;
                        CurrentSensor++;
                    }

                    var normalDist = new Normal(Average, Deviation);
                    var time = normalDist.Sample();

                    Thread.Sleep((int) time);
                    _clockTime += DateTime.Now - _clockTime;
                }
            });
            
            _clockThread.Start();
        }

        public void Start()
        {
            mrse.Set();
        }

        public void Stop()
        {
            mrse.Reset();
        }

        public void TriggerSensor(int sensorId)
        {
            TimingTriggered?.Invoke(CurrentSensor, _clockTime);
        }
    }
}