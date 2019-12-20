using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hurace.Core.Logic;
using Hurace.Core.Logic.RaceBaseDataService;
using Hurace.Core.Logic.RaceService;
using Hurace.Core.Logic.SeasonService;
using Hurace.RaceControl.Views.Windows;

namespace Hurace.RaceControl.ViewModels.PageViewModels
{
    public class MainPageViewModel : IPageViewModel
    {
        private readonly Func<IPageViewModel, Task> _changePageFunc;
        public ICommand SelectPageCommand { get; set; }
        public ICommand LaunchSimulatorCommand { get; set; }
        private readonly RacePageViewModel _racePageViewModel;

        public MainPageViewModel(Func<IPageViewModel, Task> changePageFunc)
        {
            var provider = ServiceProvider.Instance;
            _racePageViewModel = new RacePageViewModel(provider.Resolve<IRaceBaseDataService>(),
                                                       provider.Resolve<ISeasonService>());
            _changePageFunc = changePageFunc;

            SelectPageCommand = new RelayCommand<int>(SelectPage);
            LaunchSimulatorCommand = new RelayCommand(LaunchSimulator);
        }

        private static void LaunchSimulator() => new SimulatorWindow().Show();

        private void SelectPage(int pageIndex) =>
            _changePageFunc?.Invoke(pageIndex switch
            {
                0 => (IPageViewModel) _racePageViewModel,
                _ => this
            });

        public Task SetupAsync() => Task.CompletedTask; //NOOP
    }
}