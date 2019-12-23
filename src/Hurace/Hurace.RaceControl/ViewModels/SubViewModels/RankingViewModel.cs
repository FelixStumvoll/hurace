using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic.ActiveRaceControlService.Service;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.RaceStatService;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.SubViewModels
{
    public class RankingViewModel : NotifyPropertyChanged
    {
        private readonly IRaceStatService _statService;
        private readonly IActiveRaceControlService _activeRaceControlService;
        private RaceRanking _selectedRaceRanking;

        public ObservableCollection<RaceRanking> Ranking { get; set; } = new ObservableCollection<RaceRanking>();

        public RaceRanking SelectedRaceRanking
        {
            get => _selectedRaceRanking;
            set => Set(ref _selectedRaceRanking, value);
        }

        public RankingViewModel(IActiveRaceControlService activeRaceControlService, IRaceStatService statService)
        {
            _activeRaceControlService = activeRaceControlService;
            _statService = statService;
            _activeRaceControlService.OnLateDisqualification += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            _activeRaceControlService.OnSkierCancelled += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            _activeRaceControlService.OnCurrentSkierDisqualified += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            _activeRaceControlService.OnSkierFinished += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
        }

        public async Task InitializeAsync() => await LoadRanking();

        private async Task LoadRanking() =>
            Ranking.Repopulate(await _statService.GetRankingForRace(_activeRaceControlService.RaceId));
    }
}