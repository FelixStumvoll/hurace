using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic.ActiveRaceControlService.Service;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.RaceStartListService;
using Hurace.Core.Logic.RaceStatService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.SubViewModels
{
    //todo clear RaceTime after skier
    public class CurrentSkierViewModel : NotifyPropertyChanged
    {
        private readonly IActiveRaceControlService _activeRaceControlService;
        private readonly IRaceStatService _statService;
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
            IRaceStartListService startListService)
        {
            _activeRaceControlService = activeRaceControlService;
            _statService = statService;
            _stopwatch = new RaceStopwatch();
            _stopwatch.OnTimerElapsed += timespan => RaceTime = timespan;

            _activeRaceControlService.OnSplitTime += timeData =>
                UiExecutor.ExecuteInUiThreadAsync(() => OnSplitTime(timeData));
            _activeRaceControlService.OnSkierFinished += finishedSkier => UiExecutor.ExecuteInUiThreadAsync(() =>
            {
                CurrentSkier = finishedSkier;
                _stopwatch.Reset();
            });
            _activeRaceControlService.OnSkierStarted += startList =>
                UiExecutor.ExecuteInUiThreadAsync(() =>
                {
                    try
                    {
                        CurrentSkier = startList;
                        _stopwatch.Start();
                        SplitTimeList.Clear();
                    }
                    catch (Exception)
                    {
                        ErrorNotifier.OnLoadError();
                    }
                });
            _activeRaceControlService.OnCurrentSkierDisqualified += async startList =>
                await UiExecutor.ExecuteInUiThreadAsync(async () =>
                {
                    CurrentSkier = await startListService.GetStartListById(CurrentSkier.SkierId, CurrentSkier.RaceId);
                    _stopwatch.Reset();
                });
            
            _activeRaceControlService.OnRaceCancelled += race => UiExecutor.ExecuteInUiThreadAsync(() =>
            {
                _stopwatch.Reset();
            });
        }


        private async Task LoadPossiblePosition() =>
            Position = await _activeRaceControlService.GetPossiblePositionForCurrentSkier();

        private async Task OnSplitTime(TimeData timeData)
        {
            try
            {
                await LoadPossiblePosition();
                _stopwatch.StartTime ??= timeData.SkierEvent.RaceData.EventDateTime;
                var difference = await _statService.GetDifferenceToLeader(timeData);
                if (difference == null) return;

                SplitTimeList.Add(new TimeDifference(timeData, difference.Value));
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }


        public async Task InitializeAsync()
        {
            try
            {
                CurrentSkier = await _activeRaceControlService.GetCurrentSkier();
                if (CurrentSkier != null)
                {
                    if (SplitTimeList.Count > 1)
                        Position = await _activeRaceControlService.GetPossiblePositionForCurrentSkier();

                    _stopwatch.StartTime =
                        await _statService.GetStartTimeForSkier(CurrentSkier.SkierId, CurrentSkier.RaceId);
                    SplitTimeList.Repopulate(
                        await _statService.GetTimeDataForSkierWithDifference(
                            CurrentSkier.SkierId, CurrentSkier.RaceId));
                    _stopwatch.Start();
                }
                else Position = null;
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }
    }
}