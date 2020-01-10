using System;
using System.Windows.Threading;

namespace Hurace.RaceControl.ViewModels.Util
{
    public class RaceStopwatch
    {
        private readonly DispatcherTimer _dispatcherTimer;

        public event Action<TimeSpan> OnTimerElapsed;

        public DateTime? StartTime { get; set; }

        public bool Running { get; set; }

        public RaceStopwatch()
        {
            _dispatcherTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};
            _dispatcherTimer.Tick += (sender, args) => OnTick();
        }

        private void OnTick()
        {
            if (StartTime == null) return;
            OnTimerElapsed?.Invoke(StartTime.Value - DateTime.Now);
        }

        public void Start()
        {
            _dispatcherTimer.Start();
            Running = true;
        }

        public void Reset()
        {
            _dispatcherTimer.Stop();
            StartTime = null;
            Running = false;
        }
    }
}