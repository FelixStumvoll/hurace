using System.Windows;
using Autofac;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Service;
// using Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver;
using Hurace.Core.Logic.Services.RaceStatService;
using Hurace.RaceControl.ViewModels.SubViewModels;

namespace Hurace.RaceControl.Views.Windows
{
    public partial class RankingWindow
    {
        public RankingWindow(int raceId)
        {
            var container = ((App) Application.Current).Container;
            var activeRaceResolver = container.Resolve<IActiveRaceResolver>();
            var vm = container.Resolve<RankingViewModel>(
                new TypedParameter(typeof(IActiveRaceControlService), activeRaceResolver[raceId]));
            InitializeComponent();
            Loaded += async (sender, args) => await vm.InitializeAsync();
        }
    }
}