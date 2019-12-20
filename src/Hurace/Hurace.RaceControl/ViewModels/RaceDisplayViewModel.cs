using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.Views.Windows;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceDisplayViewModel : NotifyPropertyChanged
    {
        public ICommand OpenCurrentSkierWindowCommand { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }
        public RaceDisplayViewModel(SharedRaceStateViewModel raceState)
        {
            RaceState = raceState;
            OpenCurrentSkierWindowCommand = new RelayCommand(OpenCurrentSkierWindow);
        }

        private void OpenCurrentSkierWindow()
        {
            new CurrentSkierWindow(RaceState.Race.Id).Show();
        }
    }
}