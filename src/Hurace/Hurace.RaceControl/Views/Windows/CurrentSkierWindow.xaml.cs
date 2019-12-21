using Hurace.Core.Logic;
using Hurace.Core.Logic.ActiveRaceControlService.Resolver;
using Hurace.Core.Logic.RaceBaseDataService;
using Hurace.Core.Logic.RaceStartListService;
using Hurace.Core.Logic.RaceStatService;
using Hurace.RaceControl.ViewModels.SubViewModels;

namespace Hurace.RaceControl.Views.Windows
{
    public partial class CurrentSkierWindow
    {
        public CurrentSkierWindow(int raceId)
        {
            var vm = new CurrentSkierViewModel(ActiveRaceResolver.Instance[raceId],
                                               ServiceProvider.Instance.Resolve<IRaceStatService>(),
                                               ServiceProvider.Instance.Resolve<IRaceStartListService>()
            );
            DataContext = vm;
            InitializeComponent();
            Loaded += async (sender, args) => await vm.InitializeAsync();
        }
    }
}