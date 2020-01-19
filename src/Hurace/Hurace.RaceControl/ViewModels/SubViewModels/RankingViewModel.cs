using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Core.Interface.Entities;
using Hurace.Dal.Domain;
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
        private StartList _lastSkier;

        public ObservableCollection<RaceRanking> Ranking { get; set; } = new ObservableCollection<RaceRanking>();

        public RaceRanking SelectedRaceRanking
        {
            get => _selectedRaceRanking;
            set => Set(ref _selectedRaceRanking, value);
        }

        public StartList LastSkier
        {
            get => _lastSkier;
            set => Set(ref _lastSkier, value);
        }

        public RankingViewModel(IActiveRaceControlService activeRaceControlService, IRaceStatService statService)
        {
            static async Task ExceptionWrapper(Func<Task> func)
            {
                try
                {
                    await func();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            _raceId = activeRaceControlService.RaceId;
            _statService = statService;
            activeRaceControlService.OnLateDisqualification += _ =>
                UiExecutor.ExecuteInUiThreadAsync(() => ExceptionWrapper(LoadRanking));
            activeRaceControlService.OnSkierCancelled += skier =>
                UiExecutor.ExecuteInUiThreadAsync(() => ExceptionWrapper(() => LoadRankingWithSkier(skier)));
            activeRaceControlService.OnCurrentSkierDisqualified += dqSkier => UiExecutor.ExecuteInUiThreadAsync(
                () => ExceptionWrapper(() => LoadRankingWithSkier(dqSkier)));
            activeRaceControlService.OnSkierFinished += finishedSkier =>
                UiExecutor.ExecuteInUiThreadAsync(() => ExceptionWrapper(() => LoadRankingWithSkier(finishedSkier)));
        }

        public async Task InitializeAsync() => await LoadRanking();

        private async Task LoadRankingWithSkier(StartList lastSkier)
        {
            await LoadRanking();
            LastSkier = lastSkier;
        }

        private async Task LoadRanking() =>
            Ranking.Repopulate(await _statService.GetRankingForRace(_raceId));
    }
}