﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Simulation;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
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
        private MockRaceClock _clock;
        private ICommand _startClockCommand;
        private ICommand _pauseClockCommand;
        private ICommand _skipNextSensorCommand;
        private ICommand _restartSenorCommand;
        private ICommand _triggerSensorCommand;

        public ObservableCollection<SensorEntry> SensorEntries { get; set; } = new ObservableCollection<SensorEntry>();
        public int SensorToTrigger { get; set; }

        public MockRaceClock Clock
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

        public bool Running
        {
            get => _running;
            set => Set(ref _running, value);
        }

        public bool Enabled
        {
            get => _enabled;
            set => Set(ref _enabled, value);
        }

        public async Task InitializeAsync()
        {
            var resolvedRace = await RaceClockProvider.Instance.GetRaceClock();
            if (!(resolvedRace is MockRaceClock mockRaceClock))
            {
                MessageBox.Show("Simulator kann nicht gestartet werden",
                                "Fehler",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            
            Clock = mockRaceClock;
            Clock.TimingTriggered += (id, time) => Console.WriteLine($"{id}: {time}");
            Clock.TimingTriggered += (id, time) =>
                Application
                    .Current
                    ?.Dispatcher
                    ?.Invoke(() => SensorEntries.Add(new SensorEntry {DateTime = time, SensorId = id}));

            Running = Clock.Running;
            SetupCommands();
        }

        private void SetupCommands()
        {
            StartClockCommand = new ActionCommand(_ =>
            {
                Clock.Start();
                Running = true;
            }, _ => !Clock.Running);
            PauseClockCommand = new ActionCommand(_ =>
            {
                Clock.Stop();
                Running = false;
            }, _ => Clock.Running);
            SkipNextSensorCommand = new ActionCommand(_ => Clock.SkipNext());
            RestartSenorCommand = new ActionCommand(_ => Clock.Reset());
            TriggerSensorCommand = new ActionCommand(_ => Clock.TriggerSensor(SensorToTrigger),
                                                     _ => SensorToTrigger >= 0 && 
                                                          SensorToTrigger <= Clock.MaxSensor);
        }
    }
}