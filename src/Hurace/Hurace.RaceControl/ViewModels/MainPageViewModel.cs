using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.RaceCrud;
using Hurace.Core.Simulation;
using Hurace.RaceControl.Pages;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.Windows;

namespace Hurace.RaceControl.ViewModels
{
    public class MainPageViewModel : IPageViewModel
    {
        private readonly Func<IPageViewModel, Task> _changePage;
        public ICommand SelectPageCommand { get; set; }
        public ICommand LaunchSimulatorCommand { get; set; }
        private readonly RacePageViewModel _racePageViewModel;

        public MainPageViewModel(Func<IPageViewModel, Task> changePage)
        {
            var provider = ServiceProvider.Instance;
            _racePageViewModel = new RacePageViewModel(provider.ResolveService<IRaceService>());
            _changePage = changePage;
            SelectPageCommand = new ActionCommand(index => SelectPage((int) index));
            LaunchSimulatorCommand = new ActionCommand(_ => LaunchSimulator());
        }

        private void LaunchSimulator()
        {
            new SimulatorWindow().Show();
        }
        
        private void SelectPage(int pageIndex) =>
            _changePage?.Invoke(pageIndex switch
            {
                0 => (IPageViewModel) _racePageViewModel,
                _ => this
            });

        public Task SetupAsync() => Task.CompletedTask;
    }
}