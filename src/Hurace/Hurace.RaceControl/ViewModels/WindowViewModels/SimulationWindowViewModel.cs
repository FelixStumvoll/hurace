using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Simulation;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.WindowViewModels
{
    public class SimulationWindowViewModel : NotifyPropertyChanged
    {
        private MockRaceClock _clock;
        private bool _enabled;

        public ICommand StartClockCommand { get; set; }
        public ICommand PauseClockCommand { get; set; }
        
        public bool Enabled
        {
            get => _enabled;
            set => Set(ref _enabled, value);
        }

        public async Task InitializeAsync()
        {
            var resolvedRace = await RaceClockProvider.Instance.GetRaceClock();
            if (resolvedRace is MockRaceClock mockRaceClock) _clock = mockRaceClock;
            else
            {
                MessageBox.Show("Simulator kann nicht gestartet werden",
                                "Fehler",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            
            _clock.Start();
            _clock.TimingTriggered += (id, time) => Console.WriteLine($"{id}: {time}");
        }
    }
}