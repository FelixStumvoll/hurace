using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.SubViewModels;
using Hurace.RaceControl.ViewModels.Util;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.RaceControl.ViewModels.RaceControlViewModels
{
    public class ActiveRaceControlViewModel : NotifyPropertyChanged, IRaceControlViewModel
    {
        private readonly IActiveRaceControlService _activeRaceControlService;
        private readonly IRaceStartListService _startListService;
        private readonly IActiveRaceService _activeRaceService;
        private bool _eventsSetup;
        private StartList _currentSkier;
        private SharedRaceStateViewModel RaceState { get; set; }
        public CurrentSkierViewModel CurrentSkierViewModel { get; set; }
        public RankingViewModel RankingViewModel { get; set; }
        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();
        public AsyncCommand ReadyTrackCommand { get; set; }
        public ICommand CancelSkierCommand { get; set; }
        public AsyncCommand DisqualifyCurrentSkierCommand { get; set; }
        public ICommand CancelRaceCommand { get; set; }
        public ICommand EndRaceCommand { get; set; }
        public ICommand DisqualifyLateCommand { get; set; }

        public ActiveRaceControlViewModel(SharedRaceStateViewModel raceState,
            IActiveRaceControlService activeRaceControlService, IRaceStartListService startListService,
            Func<IActiveRaceControlService, CurrentSkierViewModel> currentSkierVmFactory,
            Func<IActiveRaceControlService, RankingViewModel> rankingVmFactory, IActiveRaceService activeRaceService)
        {
            RaceState = raceState;
            _activeRaceControlService = activeRaceControlService;
            _startListService = startListService;
            _activeRaceService = activeRaceService;
            CurrentSkierViewModel = currentSkierVmFactory(_activeRaceControlService);
            RankingViewModel = rankingVmFactory(_activeRaceControlService);
            SetupCommands();
        }

        private void SetupCommands()
        {
            ReadyTrackCommand = new AsyncCommand(ReadyTrack,
                                                 () =>
                                                     (_currentSkier == null || _currentSkier != null &&
                                                      (_currentSkier.StartStateId ==
                                                       (int) StartState.Finished ||
                                                       _currentSkier.StartStateId ==
                                                       (int) StartState.Disqualified)) &&
                                                     StartList.Any());
            CancelSkierCommand = new AsyncCommand<int>(async skierId => await CancelSkier(skierId));
            CancelRaceCommand = new AsyncCommand(CancelRace, () =>
                                                     RaceState.Race.RaceStateId ==
                                                     (int) Dal.Domain.Enums.RaceState.Running);
            EndRaceCommand =
                new AsyncCommand(
                    EndRace,
                    () => (_currentSkier == null ||
                           _currentSkier.StartStateId != (int) Dal.Domain.Enums.RaceState.Finished) &&
                          StartList.Count == 0 &&
                          RaceState.Race.RaceStateId ==
                          (int) Dal.Domain.Enums.RaceState.Running);
            DisqualifyCurrentSkierCommand = new AsyncCommand(
                async () => await _activeRaceControlService.DisqualifyCurrentSkier(), () =>
                    _currentSkier != null &&
                    _currentSkier.StartStateId == (int) StartState.Running);

            DisqualifyLateCommand = new AsyncCommand(() => _activeRaceControlService.DisqualifyFinishedSkier(
                                                         RankingViewModel.SelectedRaceRanking.StartList.SkierId), () =>
                                                     {
                                                         var selectedRanking = RankingViewModel
                                                             .SelectedRaceRanking;
                                                         return selectedRanking != null &&
                                                                selectedRanking.StartList.StartStateId ==
                                                                (int) StartState.Finished;
                                                     });
        }

        public async Task SetupAsync()
        {
            try
            {
                if (!_eventsSetup) SetupRaceEvents();
                _currentSkier = await _activeRaceService.GetCurrentSkier(RaceState.Race.Id);
                await LoadStartList();
                await CurrentSkierViewModel.InitializeAsync();
                await RankingViewModel.InitializeAsync();
            }
            catch (Exception)
            {
                MessageBoxUtil.Error("Fehler beim Laden der Daten");
            }
        }

        private async Task LoadStartList() =>
            StartList.Repopulate(await _activeRaceService.GetRemainingStartList(RaceState.Race.Id));

        private static void InvokeButtonCanExecuteChanged() => AsyncCommand.RaiseCanExecuteChanged();

        private void SetupRaceEvents()
        {
            static async Task ExceptionWrapper(Func<Task> func, string errorMessage)
            {
                try
                {
                    await func();
                }
                catch (Exception)
                {
                    MessageBoxUtil.Error(errorMessage);
                }
            }

            _activeRaceControlService.OnSkierStarted += async startList => await UiExecutor.ExecuteInUiThreadAsync(
                async () => await ExceptionWrapper(async () =>
                {
                    await LoadStartList();
                    _currentSkier = startList;
                    InvokeButtonCanExecuteChanged();
                }, "Startliste konnte nicht geladen werden"));
            _activeRaceControlService.OnSkierCancelled +=
                async _ => await UiExecutor.ExecuteInUiThreadAsync(
                    async () => await ExceptionWrapper(LoadStartList, "Startliste konnte nicht geladen werden"));
            _activeRaceControlService.OnCurrentSkierDisqualified += async _ =>
                await UiExecutor.ExecuteInUiThreadAsync(async () => await ExceptionWrapper(async () =>
                {
                    _currentSkier =
                        await _startListService.GetStartListById(_currentSkier.SkierId, _currentSkier.RaceId);
                    InvokeButtonCanExecuteChanged();
                }, "Startliste konnte nicht geladen werden"));
            _activeRaceControlService.OnSkierFinished += finishedSkier =>
                UiExecutor.ExecuteInUiThread(() =>
                {
                    _currentSkier = finishedSkier;
                    InvokeButtonCanExecuteChanged();
                });

            _eventsSetup = true;
        }

        private async Task ReadyTrack()
        {
            try
            {
                await _activeRaceControlService.EnableRaceForSkier();
                InvokeButtonCanExecuteChanged();
            }
            catch (Exception)
            {
                MessageBoxUtil.Error("Strecke konnte nicht freigegeben werden");
            }
        }

        private async Task CancelSkier(int skierId)
        {
            if (MessageBox.Show("Rennfahrer entfernen?\nDer Fahrer kann danach nicht mehr antreten",
                                "Fahrer entfernen", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;

            try
            {
                await _activeRaceControlService.CancelSkier(skierId);
                await LoadStartList();
            }
            catch (Exception)
            {
                MessageBoxUtil.Error("Rennläufer konnte nicht entfernt werden");
            }
        }

        private async Task CancelRace()
        {
            if (MessageBox.Show("Rennen abbrechen ?\nDas Rennen kann danach nicht mehr fortgesetzt werden",
                                "Abbrechen ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;

            try
            {
                await _activeRaceControlService.CancelRace();
            }
            catch (Exception)
            {
                MessageBoxUtil.Error("Rennen konnte nicht abgebrochen werden");
            }
        }

        private async Task EndRace()
        {
            if (MessageBox.Show("Rennen beenden ?",
                                "Beenden ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;

            try
            {
                await _activeRaceControlService.EndRace();
            }
            catch (Exception)
            {
                MessageBoxUtil.Error("Rennen konnte nicht beendet werden");
            }
        }
    }
}