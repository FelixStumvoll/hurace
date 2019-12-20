using Hurace.Core.Api;
using Hurace.Core.Api.ActiveRaceControlService.Resolver;
using Hurace.Core.Api.RaceService;
using Hurace.RaceControl.ViewModels.SubViewModels;

namespace Hurace.RaceControl.Views.Windows
{
    public partial class CurrentSkierWindow
    {
        public CurrentSkierWindow(int raceId)
        {
            var vm = new CurrentSkierViewModel(ServiceProvider.Instance.ResolveService<IRaceService>(),
                                               ActiveRaceResolver.Instance[raceId]);
            DataContext = vm;
            InitializeComponent();
            Loaded += async (sender, args) => await vm.InitializeAsync();
        }
    }
}