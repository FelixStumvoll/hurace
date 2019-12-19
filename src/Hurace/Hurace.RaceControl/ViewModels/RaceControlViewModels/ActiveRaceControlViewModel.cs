using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Hurace.Core.Api.Models;
using Hurace.Core.Api.RaceControlService.Resolver;
using Hurace.Core.Api.RaceControlService.Service;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.Util;
using Hurace.RaceControl.ViewModels.ViewModelInterfaces;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.RaceControl.ViewModels.RaceControlViewModels
{
    public class ActiveRaceControlViewModel : NotifyPropertyChanged, IRaceControlViewModel, ICurrentSkierVm,
        ISplitTimeVm
    {
        private readonly IActiveRaceControlService _activeRaceControlService;
        private StartList _currentSkier;
        private readonly IRaceService _logic;
        private bool _eventsSetup;
        private int? _position;
        private TimeSpan? _raceTime;
        public RaceStopwatch Stopwatch { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }
        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();

        public ObservableCollection<TimeDifference> SplitTimeList { get; set; } =
            new ObservableCollection<TimeDifference>();

        public ObservableCollection<RaceRanking> Ranking { get; set; } = new ObservableCollection<RaceRanking>();

        public AsyncCommand ReadyTrackCommand { get; set; }
        public ICommand CancelSkierCommand { get; set; }
        public AsyncCommand DisqualifyCurrentSkierCommand { get; set; }
        public ICommand CancelRaceCommand { get; set; }

        public StartList CurrentSkier
        {
            get => _currentSkier;
            set => Set(ref _currentSkier, value);
        }

        public int? Position
        {
            get => _position;
            set => Set(ref _position, value);
        }

        public TimeSpan? RaceTime
        {
            get => _raceTime;
            set => Set(ref _raceTime, value);
        }

        public ActiveRaceControlViewModel(SharedRaceStateViewModel raceState, IRaceService logic,
            IActiveRaceControlService activeRaceControlService)
        {
            RaceState = raceState;
            _logic = logic;
            _activeRaceControlService = activeRaceControlService;
            Stopwatch = new RaceStopwatch();
            Stopwatch.OnTimerElapsed += timespan => RaceTime = timespan;
            SetupCommands();
        }

        private void SetupCommands()
        {
            ReadyTrackCommand = new AsyncCommand(async _ => await ReadyTrack(),
                                                 _ =>
                                                     (CurrentSkier == null || CurrentSkier != null &&
                                                      (CurrentSkier.StartStateId ==
                                                       (int) Dal.Domain.Enums.RaceState.Finished ||
                                                       CurrentSkier.StartStateId ==
                                                       (int) Dal.Domain.Enums.RaceState.Disqualified)) &&
                                                     StartList.Any());
            CancelSkierCommand = new AsyncCommand(async skierId => await CancelSkier((int) skierId));
            CancelRaceCommand = new AsyncCommand(async _ => await CancelRace());
            DisqualifyCurrentSkierCommand = new AsyncCommand(async _ =>
            {
                Stopwatch.Reset();
                await _activeRaceControlService
                    .DisqualifyCurrentSkier();
            }, _ => CurrentSkier != null &&
                    CurrentSkier.StartStateId == (int) Dal.Domain.Enums.RaceState.Running);
        }

        public async Task SetupAsync()
        {
            if (!_eventsSetup) SetupRaceEvents();
            await LoadData();
            if (CurrentSkier != null) Stopwatch.Start();
        }

        private async Task LoadData()
        {
            try
            {
                await LoadCurrentSkier();
                await LoadStartList();
                await LoadSplitTimes();
                await LoadRanking();
                if (CurrentSkier != null)
                {
                    if (SplitTimeList.Count > 1)
                        Position = await _activeRaceControlService.GetPossiblePositionForCurrentSkier();
                    Stopwatch.StartTime = await _logic.GetStartTimeForSkier(CurrentSkier.SkierId, RaceState.Race.Id);
                }
                else
                {
                    Position = null;
                    RaceTime = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                ErrorNotifier.OnLoadError();
            }
        }

        private async Task LoadStartList() =>
            StartList.Repopulate(await _activeRaceControlService.GetRemainingStartList());

        private async Task LoadCurrentSkier() => CurrentSkier = await _activeRaceControlService.GetCurrentSkier();

        private async Task LoadSplitTimes()
        {
            DateTime? startTime = null;
            if (CurrentSkier != null)
            {
                startTime = await _logic.GetStartTimeForSkier(CurrentSkier.SkierId, RaceState.Race.Id);
                SplitTimeList
                    .Repopulate(
                        await _logic.GetTimeDataForSkierWithDifference(CurrentSkier.SkierId, RaceState.Race.Id));
            }
            else SplitTimeList.Clear();

            Stopwatch.StartTime = startTime;
        }

        private async Task LoadRanking() =>
            Ranking.Repopulate(await _logic.GetRankingForRace(RaceState.Race.Id));

        private async Task LoadPossiblePosition() =>
            Position = await _activeRaceControlService.GetPossiblePositionForCurrentSkier();
        

        private void InvokeButtonCanExecuteChanged()
        {
            ReadyTrackCommand.RaiseCanExecuteChanged();
            DisqualifyCurrentSkierCommand.RaiseCanExecuteChanged();
        }

        private void SetupRaceEvents()
        {
            _activeRaceControlService.OnSkierStarted += async startList =>
            {
                await UiExecutor.ExecuteInUiThreadAsync(async () =>
                {
                    try
                    {
                        CurrentSkier = startList;
                        await LoadStartList();
                        Stopwatch.Start();
                        InvokeButtonCanExecuteChanged();
                    }
                    catch (Exception)
                    {
                        ErrorNotifier.OnLoadError();
                    }
                });
            };
            _activeRaceControlService.OnSplitTime +=
                async timeData => await UiExecutor.ExecuteInUiThreadAsync(() => OnSplitTime(timeData));
            _activeRaceControlService.OnSkierCanceled += async _ => await UiExecutor.ExecuteInUiThreadAsync(LoadStartList);
            _activeRaceControlService.OnLateDisqualification += async _ => await UiExecutor.ExecuteInUiThreadAsync(LoadRanking);
            _activeRaceControlService.OnCurrentSkierDisqualified += async _ =>
            {
                await UiExecutor.ExecuteInUiThreadAsync(async () =>
                {
                    await LoadRanking();
                    CurrentSkier = await _logic.GetStartListById(CurrentSkier.SkierId, CurrentSkier.RaceId);
                    Stopwatch.Reset();
                    InvokeButtonCanExecuteChanged();
                });
            };
            _activeRaceControlService.OnSkierFinished += async finishedSkier =>
                await UiExecutor.ExecuteInUiThreadAsync(async () =>
                {
                    CurrentSkier = finishedSkier;
                    Stopwatch.Reset();
                    await LoadRanking();
                    InvokeButtonCanExecuteChanged();
                });
            _eventsSetup = true;
        }

        private async Task ReadyTrack()
        {
            try
            {
                Position = null;
                RaceTime = null;
                await _activeRaceControlService.EnableRaceForSkier();
                SplitTimeList.Clear();
                InvokeButtonCanExecuteChanged();
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }

        private async Task OnSplitTime(TimeData timeData)
        {
            try
            {
                await LoadPossiblePosition();
                Stopwatch.StartTime ??= timeData.SkierEvent.RaceData.EventDateTime;
                var difference = await _logic.GetDifferenceToLeader(timeData);
                if (difference == null) return;

                SplitTimeList.Add(new TimeDifference
                {
                    TimeData = timeData,
                    DifferenceToLeader = difference.Value
                });
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
    }
}