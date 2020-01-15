using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Service;
using Hurace.Core.Logic.Services.ActiveRaceService;
using Hurace.Core.Logic.Services.RaceStartListService;
using Hurace.Core.Logic.Services.RaceStatService;
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
                                                       (int)StartState.Finished ||
                                                       _currentSkier.StartStateId ==
                                                       (int)StartState.Disqualified)) &&
                                                     StartList.Any());
            CancelSkierCommand = new AsyncCommand<int>(async skierId => await CancelSkier(skierId));
            CancelRaceCommand = new AsyncCommand(CancelRace, () =>
                                                     RaceState.Race.RaceStateId ==
                                                     (int)Dal.Domain.Enums.RaceState.Running);
            EndRaceCommand =
                new AsyncCommand(
                    EndRace,
                    () => (_currentSkier == null ||
                           _currentSkier.StartStateId != (int)Dal.Domain.Enums.RaceState.Finished) &&
                          StartList.Count == 0 &&
                          RaceState.Race.RaceStateId ==
                          (int)Dal.Domain.Enums.RaceState.Running);
            DisqualifyCurrentSkierCommand = new AsyncCommand(
                async () => { await _activeRaceControlService.DisqualifyCurrentSkier(); }, () =>
                    _currentSkier != null &&
                    _currentSkier.StartStateId == (int)StartState.Running);

            DisqualifyLateCommand = new AsyncCommand(() => _activeRaceControlService.DisqualifyFinishedSkier(
                                                         RankingViewModel.SelectedRaceRanking.StartList.SkierId), () =>
                                                     {
                                                         var selectedRanking = RankingViewModel
                                                             .SelectedRaceRanking;
                                                         return selectedRanking != null &&
                                                                selectedRanking.StartList.StartStateId ==
                                                                (int)StartState.Finished;
                                                     });
        }

        public async Task SetupAsync()
        {
            if (!_eventsSetup) SetupRaceEvents();
            await LoadData();
            await CurrentSkierViewModel.InitializeAsync();
            await RankingViewModel.InitializeAsync();
        }

        private async Task LoadData()
        {
            try
            {
                _currentSkier = await _activeRaceService.GetCurrentSkier(RaceState.Race.Id);
                await LoadStartList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                ErrorNotifier.OnLoadError();
            }
        }

        private async Task LoadStartList() =>
            StartList.Repopulate(await _activeRaceService.GetRemainingStartList(RaceState.Race.Id));

        private static void InvokeButtonCanExecuteChanged() => AsyncCommand.RaiseCanExecuteChanged();

        private void SetupRaceEvents()
        {
            _activeRaceControlService.OnSkierStarted += async startList =>
            {
                await UiExecutor.ExecuteInUiThreadAsync(async () =>
                {
                    try
                    {
                        await LoadStartList();
                        _currentSkier = startList;
                        InvokeButtonCanExecuteChanged();
                    }
                    catch (Exception)
                    {
                        ErrorNotifier.OnLoadError();
                    }
                });
            };
            _activeRaceControlService.OnSkierCancelled +=
                async _ => await UiExecutor.ExecuteInUiThreadAsync(LoadStartList);
            _activeRaceControlService.OnCurrentSkierDisqualified += async _ =>
                await UiExecutor.ExecuteInUiThreadAsync(async () =>
                {
                    _currentSkier =
                        await _startListService.GetStartListById(_currentSkier.SkierId, _currentSkier.RaceId);
                    InvokeButtonCanExecuteChanged();
                });
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
                ErrorNotifier.OnLoadError();
            }
        }

        private async Task CancelSkier(int skierId)
        {
            if (MessageBox.Show("Rennfahrer entfernen?\nDer Fahrer kann danach nicht mehr antreten",
                                "Fahrer entfernen", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;

            await _activeRaceControlService.CancelSkier(skierId);
            await LoadStartList();
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
                ErrorNotifier.OnSaveError();
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
                ErrorNotifier.OnSaveError();
            }
        }
    }
}