using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Hurace.Core.Api.ActiveRaceControlService.Service;
using Hurace.Core.Api.Models;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.SubViewModels
{
    public class CurrentSkierViewModel : NotifyPropertyChanged
    {
        private readonly IActiveRaceControlService _activeRaceControlService;
        private readonly IRaceService _logic;
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

        public CurrentSkierViewModel(IRaceService logic, IActiveRaceControlService activeRaceControlService)
        {
            _logic = logic;
            _activeRaceControlService = activeRaceControlService;
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
                    CurrentSkier = await _logic.GetStartListById(CurrentSkier.SkierId, CurrentSkier.RaceId);
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


        public async Task InitializeAsync()
        {
            try
            {
                CurrentSkier = await _activeRaceControlService.GetCurrentSkier();
                if (CurrentSkier != null)
                {
                    if (SplitTimeList.Count > 1)
                        Position = await _activeRaceControlService.GetPossiblePositionForCurrentSkier();
                    
                    _stopwatch.StartTime = await _logic.GetStartTimeForSkier(CurrentSkier.SkierId, CurrentSkier.RaceId);
                    SplitTimeList.Repopulate(
                            await _logic.GetTimeDataForSkierWithDifference(CurrentSkier.SkierId, CurrentSkier.RaceId));
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