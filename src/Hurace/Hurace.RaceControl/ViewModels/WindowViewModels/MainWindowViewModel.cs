using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.PageViewModels;
using Hurace.RaceControl.Views.Windows;

namespace Hurace.RaceControl.ViewModels.WindowViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private bool _canLaunchSimulator = true;
        public RacePageViewModel RacePageViewModel { get; set; }

        public ICommand LaunchSimulatorCommand { get; set; }

        public bool CanLaunchSimulator
        {
            get => _canLaunchSimulator;
            set => Set(ref _canLaunchSimulator, value);
        }

        public MainWindowViewModel(RacePageViewModel racePageViewModel)
        {
            RacePageViewModel = racePageViewModel;
            LaunchSimulatorCommand = new RelayCommand(() =>
            {
                CanLaunchSimulator = false;
                var simWindow = new SimulatorWindow();
                simWindow.Closed += (sender, args) => CanLaunchSimulator = true;
                simWindow.Show();
            }, () => CanLaunchSimulator);
        }

        public async Task InitializeAsync() => await RacePageViewModel.SetupAsync();
    }
}