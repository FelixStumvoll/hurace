using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api.RaceControlService;
using Hurace.Core.Api.RaceControlService.Resolver;
using Hurace.Core.Api.RaceControlService.Service;
using Hurace.Core.Api.RaceService;
using Hurace.Core.Api.Util;
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
            if (newStartList.Failure)
            {
                ErrorNotifier.OnLoadError();
                return;
            }

            StartList.Clear();
            StartList.AddRange(newStartList.Value);
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

        private async Task<Result<IEnumerable<StartList>, Exception>> SetupRaceControl()
        {
            SetupRaceHooks();

            return (await _activeRaceControlService.GetCurrentSkier())
                   .AndThen(currentSkier => { CurrentSkier = currentSkier; })
                   .And(await _activeRaceControlService.GetRemainingStartList(), startList =>
                   {
                       StartList.Clear();
                       StartList.AddRange(startList);
                   }).OrElse(_ => ErrorNotifier.OnLoadError());
        }

        private async Task StartRace()
        {
            if (MessageBox.Show("Rennen kann nach dem Starten nicht mehr bearbeitet werden. Fortfahren ?",
                                "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;

            (await ActiveRaceResolver.Instance.StartRace(RaceState.Race.Id))
                .AndThen(arc => _activeRaceControlService = arc)
                .And(await _logic.GetRaceById(RaceState.Race.Id), race => RaceState.Race = race)
                .And(await SetupRaceControl())
                .OrElse(_ => ErrorNotifier.OnLoadError());
        }
    }
}