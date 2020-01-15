using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Hurace.Core.Logic;
using Hurace.Core.Simulation;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Util;


namespace Hurace.RaceControl.ViewModels.WindowViewModels
{
    public class SimulationWindowViewModel : NotifyPropertyChanged
    {
        public class SensorEntry
        {
            public int SensorId { get; set; }
            public DateTime DateTime { get; set; }
        }

        private bool _enabled = true;
        private bool _running;
        private MockRaceClockV2 _clock;
        private ICommand _startClockCommand;
        private ICommand _pauseClockCommand;
        private ICommand _skipNextSensorCommand;
        private ICommand _restartSenorCommand;
        private ICommand _triggerSensorCommand;
        private readonly IRaceClockProvider _raceClockProvider;
        private int _nextSensor;
        private int _countdown;

        public ObservableCollection<SensorEntry> SensorEntries { get; set; } = new ObservableCollection<SensorEntry>();
        public int SensorToTrigger { get; set; }

        public MockRaceClockV2 Clock
        {
            get => _clock;
            set => Set(ref _clock, value);
        }

        public ICommand StartClockCommand
        {
            get => _startClockCommand;
            set => Set(ref _startClockCommand, value);
        }

        public ICommand PauseClockCommand
        {
            get => _pauseClockCommand;
            set => Set(ref _pauseClockCommand, value);
        }

        public ICommand SkipNextSensorCommand
        {
            get => _skipNextSensorCommand;
            set => Set(ref _skipNextSensorCommand, value);
        }

        public ICommand RestartSenorCommand
        {
            get => _restartSenorCommand;
            set => Set(ref _restartSenorCommand, value);
        }

        public ICommand TriggerSensorCommand
        {
            get => _triggerSensorCommand;
            set => Set(ref _triggerSensorCommand, value);
        }

        public bool Enabled
        {
            get => _enabled;
            set => Set(ref _enabled, value);
        }

        public int NextSensor
        {
            get => _nextSensor;
            set => Set(ref _nextSensor, value);
        }

        public int Countdown
        {
            get => _countdown;
            set => Set(ref _countdown, value);
        }

        public bool Running
        {
            get => _running;
            set => Set(ref _running, value);
        }

        public SimulationWindowViewModel(IRaceClockProvider raceClockProvider)
        {
            _raceClockProvider = raceClockProvider;
        }

        public async Task InitializeAsync()
        {
            var resolvedRace = await _raceClockProvider.GetRaceClock();
            if (!(resolvedRace is MockRaceClockV2 mockRaceClock))
            {
                MessageBox.Show("Simulator kann nicht gestartet werden",
                                "Fehler",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            Clock = mockRaceClock;
            Clock.OnCountdownChanged += countdown => UiExecutor.ExecuteInUiThread(() => Countdown = countdown);
            Clock.OnNextSensorChanged += sensorNumber => UiExecutor.ExecuteInUiThread(() => NextSensor = sensorNumber);
            Clock.OnRunningChanged += state => UiExecutor.ExecuteInUiThread(() => Running = state);
            Clock.TimingTriggered += (id, time) =>
                UiExecutor.ExecuteInUiThread(() => SensorEntries.Add(new SensorEntry { DateTime = time, SensorId = id }));
            SetupCommands();
        }

        private void SetupCommands()
        {
            StartClockCommand = new RelayCommand(() =>
            {
                Running = true;
                Clock.Start();
            }, () => !Running);
            PauseClockCommand = new RelayCommand(() =>
            {
                Running = false;
                Clock.Stop();
            }, () => Running);
            SkipNextSensorCommand = new RelayCommand(() => Clock.SkipNextSensor(), () => NextSensor + 1 < Clock.SensorCount);
            RestartSenorCommand = new RelayCommand(() => Clock.Reset());
            TriggerSensorCommand = new RelayCommand(() => Clock.TriggerSensor(SensorToTrigger),
                                                    () => SensorToTrigger >= 0 &&
                                                          SensorToTrigger < Clock.SensorCount);
        }

        public void OnClose() => _clock.Stop();
    }
}