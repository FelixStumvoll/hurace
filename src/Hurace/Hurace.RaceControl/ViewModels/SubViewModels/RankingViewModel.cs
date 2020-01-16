using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Core.Interface.Entities;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.SubViewModels
{
    public class RankingViewModel : NotifyPropertyChanged
    {
        private readonly IRaceStatService _statService;
        private RaceRanking _selectedRaceRanking;
        private readonly int _raceId;

        public ObservableCollection<RaceRanking> Ranking { get; set; } = new ObservableCollection<RaceRanking>();

        public RaceRanking SelectedRaceRanking
        {
            get => _selectedRaceRanking;
            set => Set(ref _selectedRaceRanking, value);
        }

        public RankingViewModel(IActiveRaceControlService activeRaceControlService, IRaceStatService statService)
        {
            _raceId = activeRaceControlService.RaceId;
            _statService = statService;
            activeRaceControlService.OnLateDisqualification += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            activeRaceControlService.OnSkierCancelled += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            activeRaceControlService.OnCurrentSkierDisqualified += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            activeRaceControlService.OnSkierFinished += _ => UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
        }

        public async Task InitializeAsync() => await LoadRanking();

        private async Task LoadRanking() =>
            Ranking.Repopulate(await _statService.GetRankingForRace(_raceId));
    }
}