using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api.RaceControlService;
using Hurace.Core.Api.RaceControlService.Resolver;
using Hurace.Core.Api.RaceControlService.Service;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceControlViewModel : NotifyPropertyChanged
    {
        private IActiveRaceControlService _activeRaceControlService;
        private StartList _currentSkier;
        private readonly IRaceService _logic;

        public SharedRaceStateViewModel RaceState { get; set; }
        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();
        public ObservableCollection<TimeData> SkierTimeData { get; set; } = new ObservableCollection<TimeData>();
        public ICommand StartRaceCommand { get; set; }
        public ICommand ReadyTrackCommand { get; set; }
        public ICommand CancelSkier { get; set; }

        public StartList CurrentSkier
        {
            get => _currentSkier;
            set => Set(ref _currentSkier, value);
        }

        public RaceControlViewModel(SharedRaceStateViewModel raceState, IRaceService logic)
        {
            RaceState = raceState;
            _logic = logic;
            SetupCommands();
        }

        private async Task ReloadStartList()
        {
            var newStartList = await _activeRaceControlService.GetRemainingStartList();
            if (newStartList == null)
            {
                ErrorNotifier.OnLoadError();
                return;
            }
            StartList.Clear();
            StartList.AddRange(newStartList);
        }
        
        private void SetupRaceHooks()
        {
            _activeRaceControlService.OnSkierStarted += async startList =>
            {
                CurrentSkier = startList;
                await ReloadStartList();
            };
            _activeRaceControlService.OnSplitTime += async _ =>
            {
                
            };
            _activeRaceControlService.OnSkierCanceled += async _ => await ReloadStartList();
            _activeRaceControlService.OnSkierFinished += _ => { CurrentSkier = null; };
        }
        

        private void SetupCommands()
        {
            StartRaceCommand = new AsyncCommand(_ => StartRace());
            ReadyTrackCommand = new AsyncCommand(async _ => { await _activeRaceControlService.EnableRaceForSkier(); },
                                                 _ => StartList.Any());
            CancelSkier = new AsyncCommand(async skierId =>
                                               await _activeRaceControlService.CancelSkier((int) skierId));
        }

        public async Task SetupAsync()
        {
            _activeRaceControlService ??= ActiveRaceResolver.Instance[RaceState.Race.Id];
            if (_activeRaceControlService != null)
                await SetupRaceControl();
        }

        private async Task SetupRaceControl()
        {
            SetupRaceHooks();
            CurrentSkier = await _activeRaceControlService.GetCurrentSkier();
            var startList = await _activeRaceControlService.GetRemainingStartList();
            if (CurrentSkier == null || startList == null)
            {
                ErrorNotifier.OnLoadError();
                return;
            }
            StartList.Clear();
            StartList.AddRange(startList);
        }

        private async Task StartRace()
        {
            if (MessageBox.Show("Rennen kann nach dem Starten nicht mehr bearbeitet werden. Fortfahren ?",
                                "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;
            _activeRaceControlService = await ActiveRaceResolver.Instance.StartRace(RaceState.Race.Id);
            if (_activeRaceControlService == null)
            {
                ErrorNotifier.OnLoadError();
                return;
            }
            RaceState.Race = await _logic.GetRaceById(RaceState.Race.Id);
            await SetupRaceControl();
        }
    }
}