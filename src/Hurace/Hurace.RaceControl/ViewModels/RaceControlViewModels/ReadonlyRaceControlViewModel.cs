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

namespace Hurace.RaceControl.ViewModels.RaceControlViewModels
{
    public class ReadonlyRaceControlViewModel : NotifyPropertyChanged, IRaceControlViewModel
    {
        private StartList _winner;

        public StartList Winner
        {
            get => _winner;
            set => Set(ref _winner, value);
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
        }

        public async Task SetupAsync()
        {
            StartList.AddRange(await _startListService.GetStartListForRace(_raceState.Race.Id));
            Ranking.AddRange(await _raceStatService.GetRankingForRace(_raceState.Race.Id));
            Winner = Ranking[0].StartList;
        }
    }
}