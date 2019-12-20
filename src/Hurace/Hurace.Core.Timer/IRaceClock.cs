using System;

namespace Hurace.Core.Timer
{
    public delegate void TimingTriggeredHandler(int sensorId, DateTime time);
    
    public interface IRaceClock
    {
        event TimingTriggeredHandler TimingTriggered;
    }
}