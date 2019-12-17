using System;
using System.Windows.Threading;

namespace Hurace.RaceControl.ViewModels.Util
{
    public class RtTimer : NotifyPropertyChanged
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private TimeSpan _clockTime;

        public DateTime? StartTime { get; set; }
        
        public TimeSpan ClockTime
        {
            get => _clockTime;
            set => Set(ref _clockTime, value);
        }


        public RtTimer()
        {
            _dispatcherTimer = new DispatcherTimer{Interval = TimeSpan.FromMilliseconds(10)};
            _dispatcherTimer.Tick += (sender, args) => OnTick();
        }

        private void OnTick()
        {
            if (StartTime == null) return;
            ClockTime = StartTime.Value - DateTime.Now;
        }

        public void Start() => _dispatcherTimer.Start();

        public void Reset()
        {
            _dispatcherTimer.Stop();
            StartTime = null;
        }
    }
}