﻿using System.Windows.Input;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;
using Hurace.RaceControl.Windows;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceDisplayViewModel : NotifyPropertyChanged
    {
        public ICommand OpenCurrentSkierWindowCommand { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }

        public RaceDisplayViewModel(SharedRaceStateViewModel raceState)
        {
            RaceState = raceState;
            OpenCurrentSkierWindowCommand = new ActionCommand(_ => OpenCurrentSkierWindow());
        }

        private void OpenCurrentSkierWindow()
        {
            new CurrentSkierWindow(RaceState.Race.Id).Show();
        }
    }
}