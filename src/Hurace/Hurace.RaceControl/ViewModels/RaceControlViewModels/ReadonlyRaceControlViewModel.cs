using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Services.RaceStartListService;
using Hurace.Core.Logic.Services.RaceStatService;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using RaceState = Hurace.Dal.Domain.Enums.RaceState;

namespace Hurace.RaceControl.ViewModels.RaceControlViewModels
{
    public class ReadonlyRaceControlViewModel : NotifyPropertyChanged, IRaceControlViewModel
    {
        private StartList _winner;
        private bool _finished;

        public StartList Winner
        {
            get => _winner;
            set => Set(ref _winner, value);
        }

        public bool Finished
        {
            get => _finished;
            set => Set(ref _finished, value);
        }

        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();
        public ObservableCollection<RaceRanking> Ranking { get; set; } = new ObservableCollection<RaceRanking>();

        private readonly IRaceStatService _raceStatService;
        private readonly SharedRaceStateViewModel _raceState;
        private readonly IRaceStartListService _startListService;

        public ReadonlyRaceControlViewModel(IRaceStatService raceStatService, SharedRaceStateViewModel raceState,
            IRaceStartListService startListService)
        {
            _raceStatService = raceStatService;
            _raceState = raceState;
            _startListService = startListService;
            Finished = raceState.Race.RaceStateId == (int)RaceState.Finished;
        }

        public async Task SetupAsync()
        {
            StartList.Clear();
            Ranking.Clear();
            Winner = null;
            StartList.AddRange(await _startListService.GetStartListForRace(_raceState.Race.Id));
            Ranking.AddRange(await _raceStatService.GetRankingForRace(_raceState.Race.Id));
            if (Finished) Winner = Ranking[0].StartList;
        }
    }
}