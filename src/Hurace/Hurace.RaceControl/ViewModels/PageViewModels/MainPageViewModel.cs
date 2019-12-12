using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.RaceService;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.Windows;

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
            _racePageViewModel = new RacePageViewModel(provider.ResolveService<IRaceService>());
            _changePageFunc = changePageFunc;
            
            SelectPageCommand = new ActionCommand(index => SelectPage((int) index));
            LaunchSimulatorCommand = new ActionCommand(_ => LaunchSimulator());
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