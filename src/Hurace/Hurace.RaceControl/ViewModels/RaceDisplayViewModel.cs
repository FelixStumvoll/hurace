using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.Views.Windows;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceDisplayViewModel : NotifyPropertyChanged
    {
        public ICommand OpenCurrentSkierWindowCommand { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }

        public ICommand OpenRankingWindowCommand { get; set; }

        public RaceDisplayViewModel(SharedRaceStateViewModel raceState)
        {
            RaceState = raceState;
            OpenCurrentSkierWindowCommand = new RelayCommand(OpenCurrentSkierWindow);
            OpenRankingWindowCommand = new RelayCommand(OpenRankingWindow);
            
        }

        private void OpenCurrentSkierWindow() => new CurrentSkierWindow(RaceState.Race.Id).Show();

        private void OpenRankingWindow() => new RankingWindow(RaceState.Race.Id).Show();
    }
}