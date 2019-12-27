using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Services.RaceBaseDataService;
using Hurace.Core.Logic.Services.SeasonService;
using Hurace.RaceControl.Views.Windows;

namespace Hurace.RaceControl.ViewModels.PageViewModels
{
    public class MainPageViewModel : IPage
    {
        private Func<IPage, Task> ChangePageFunc { get; set; }
        public ICommand SelectPageCommand { get; set; }
        public ICommand LaunchSimulatorCommand { get; set; }
        private readonly RacePageViewModel _racePageViewModel;

        public MainPageViewModel(Func<IPage, Task> changePageFunc, RacePageViewModel racePageViewModel)
        {
            _racePageViewModel = racePageViewModel;
            ChangePageFunc = changePageFunc;

            SelectPageCommand = new RelayCommand<int>(SelectPage);
            LaunchSimulatorCommand = new RelayCommand(LaunchSimulator);
        }

        private static void LaunchSimulator() => new SimulatorWindow().Show();

        private void SelectPage(int pageIndex) =>
            ChangePageFunc?.Invoke(pageIndex switch
            {
                0 => (IPage) _racePageViewModel,
                _ => this
            });

        public Task SetupAsync() => Task.CompletedTask; //NOOP
    }
}