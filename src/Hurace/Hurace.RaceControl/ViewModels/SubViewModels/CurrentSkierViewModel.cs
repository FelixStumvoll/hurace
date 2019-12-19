using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Hurace.Core.Api.Models;
using Hurace.Core.Api.RaceControlService.Service;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Util;
using Hurace.RaceControl.ViewModels.ViewModelInterfaces;

namespace Hurace.RaceControl.ViewModels.SubViewModels
{
    public class CurrentSkierViewModel : NotifyPropertyChanged, ICurrentSkierVm, ISplitTimeVm
    {
        private readonly IActiveRaceControlService _activeRaceControlService;
        private readonly IRaceService _raceService;
        private int? _position;
        private TimeSpan? _raceTime;
        private RaceStopwatch _stopwatch;
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

        public CurrentSkierViewModel(IRaceService raceService, IActiveRaceControlService activeRaceControlService)
        {
            _raceService = raceService;
            _activeRaceControlService = activeRaceControlService;
            _stopwatch = new RaceStopwatch();

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
                    CurrentSkier = await _raceService.GetStartListById(CurrentSkier.SkierId, CurrentSkier.RaceId);
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
                var difference = await _raceService.GetDifferenceToLeader(timeData);
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
                    _stopwatch.StartTime =
                        await _raceService.GetStartTimeForSkier(CurrentSkier.SkierId, CurrentSkier.RaceId);
                }
                else
                {
                    Position = null;
                    _stopwatch = null;
                }
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }
    }
}