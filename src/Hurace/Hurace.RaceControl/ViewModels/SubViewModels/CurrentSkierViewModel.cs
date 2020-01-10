using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Service;
using Hurace.Core.Logic.Services.ActiveRaceService;
using Hurace.Core.Logic.Services.RaceStartListService;
using Hurace.Core.Logic.Services.RaceStatService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.SubViewModels
{
    public class CurrentSkierViewModel : NotifyPropertyChanged
    {
        private readonly IActiveRaceControlService _activeRaceControlService;
        private readonly IRaceStatService _statService;
        private readonly IActiveRaceService _activeRaceService;
        private int? _position;
        private TimeSpan? _raceTime;
        private readonly RaceStopwatch _stopwatch;
        private StartList _currentSkier;

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

        public ObservableCollection<TimeDifference> SplitTimeList { get; set; } =
            new ObservableCollection<TimeDifference>();

        public CurrentSkierViewModel(IActiveRaceControlService activeRaceControlService, IRaceStatService statService,
            IRaceStartListService startListService, IActiveRaceService activeRaceService)
        {
            _activeRaceControlService = activeRaceControlService;
            _statService = statService;
            _activeRaceService = activeRaceService;
            _stopwatch = new RaceStopwatch();
            _stopwatch.OnTimerElapsed += timespan => RaceTime = timespan;

            _activeRaceControlService.OnSplitTime += async timeData => await OnSplitTime(timeData);
            _activeRaceControlService.OnSkierFinished += finishedSkier =>
            {
                _stopwatch.Reset();
                UiExecutor.ExecuteInUiThread(() => { CurrentSkier = finishedSkier; });
            };
            _activeRaceControlService.OnSkierStarted += startList =>
            {
                SplitTimeList.Clear();
                UiExecutor.ExecuteInUiThread(() =>
                {
                    try
                    {
                        CurrentSkier = startList;

                        RaceTime = null;
                    }
                    catch (Exception)
                    {
                        ErrorNotifier.OnLoadError();
                    }
                });
            };
            _activeRaceControlService.OnCurrentSkierDisqualified += async startList =>
            {
                _stopwatch.Reset();
                await UiExecutor.ExecuteInUiThreadAsync(async () =>
                {
                    CurrentSkier =
                        await startListService.GetStartListById(
                            CurrentSkier.SkierId, CurrentSkier.RaceId);
                });
            };

            _activeRaceControlService.OnRaceCancelled +=
                race => _stopwatch.Reset();
        }


        private async Task LoadPossiblePosition() =>
            Position = await _activeRaceService.GetPossiblePositionForCurrentSkier(_activeRaceControlService.RaceId);

        private async Task OnSplitTime(TimeData timeData)
        {
            try
            {
                _stopwatch.StartTime ??= timeData.SkierEvent.RaceData.EventDateTime;
                if (!_stopwatch.Running) _stopwatch.Start();
                UiExecutor.ExecuteInUiThread(async () =>
                {
                    await LoadPossiblePosition();
                    var difference = await _statService.GetDifferenceToLeader(timeData);
                    if (difference == null) return;
                    SplitTimeList.Add(new TimeDifference(timeData, difference.Value));
                });
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }


        public async Task InitializeAsync()
        {
            RaceTime = null;
            try
            {
                CurrentSkier = await _activeRaceService.GetCurrentSkier(_activeRaceControlService.RaceId);
                if (CurrentSkier != null)
                {
                    if (SplitTimeList.Count > 1)
                        Position = await _activeRaceService.GetPossiblePositionForCurrentSkier(
                            _activeRaceControlService.RaceId);

                    _stopwatch.StartTime =
                        await _statService.GetStartTimeForSkier(CurrentSkier.SkierId, CurrentSkier.RaceId);
                    SplitTimeList.Repopulate(
                        await _statService.GetTimeDataForSkierWithDifference(
                            CurrentSkier.SkierId, CurrentSkier.RaceId));
                    _stopwatch.Start();
                }
                else
                {
                    Position = null;
                    SplitTimeList.Clear();
                }
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }
    }
}