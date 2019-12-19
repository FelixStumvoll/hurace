using System.Windows;
using Hurace.Core.Api;
using Hurace.Core.Api.ActiveRaceControlService.Resolver;
using Hurace.Core.Api.RaceService;
using Hurace.RaceControl.ViewModels.SubViewModels;

namespace Hurace.RaceControl.Windows
{
    public partial class RankingWindow : Window
    {
        public RankingWindow(int raceId)
        {
            var vm = new RankingViewModel(ServiceProvider.Instance.ResolveService<IRaceService>(),
                                          ActiveRaceResolver.Instance[raceId]);
            InitializeComponent();
            Loaded += async (sender, args) => await vm.InitializeAsync();
        }
    }
}