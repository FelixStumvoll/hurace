using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Api.ActiveRaceControlService.Service;
using Hurace.Core.Api.Models;
using Hurace.Core.Api.RaceService;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.SubViewModels
{
    public class RankingViewModel : NotifyPropertyChanged
    {
        private readonly IRaceService _logic;
        private readonly IActiveRaceControlService _activeRaceControlService;
        
        public ObservableCollection<RaceRanking> Ranking { get; set; } = new ObservableCollection<RaceRanking>();


        public RankingViewModel(IRaceService logic, IActiveRaceControlService activeRaceControlService)
        {
            _logic = logic;
            _activeRaceControlService = activeRaceControlService;
            _activeRaceControlService.OnLateDisqualification += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            _activeRaceControlService.OnCurrentSkierDisqualified += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            _activeRaceControlService.OnSkierFinished += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
        }
        
        public async Task InitializeAsync() => await LoadRanking();

        private async Task LoadRanking() => 
            Ranking.Repopulate(await _logic.GetRankingForRace(_activeRaceControlService.RaceId));
    }
}