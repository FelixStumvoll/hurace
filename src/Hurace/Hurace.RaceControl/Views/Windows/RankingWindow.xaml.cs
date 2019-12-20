using Hurace.Core.Logic;
using Hurace.Core.Logic.ActiveRaceControlService.Resolver;
using Hurace.Core.Logic.RaceService;
using Hurace.Core.Logic.RaceStatService;
using Hurace.RaceControl.ViewModels.SubViewModels;

namespace Hurace.RaceControl.Views.Windows
{
    public partial class RankingWindow
    {
        public RankingWindow(int raceId)
        {
            var vm = new RankingViewModel(ActiveRaceResolver.Instance[raceId],
                                          ServiceProvider.Instance.Resolve<IRaceStatService>());
            InitializeComponent();
            Loaded += async (sender, args) => await vm.InitializeAsync();
        }
    }
}