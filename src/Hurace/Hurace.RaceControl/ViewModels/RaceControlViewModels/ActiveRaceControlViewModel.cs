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
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.RaceControl.ViewModels.RaceControlViewModels
{
    public class ActiveRaceControlViewModel : NotifyPropertyChanged, IRaceControlViewModel
    {
        private readonly IActiveRaceControlService _activeRaceControlService;
        private StartList _currentSkier;
        private readonly IRaceService _logic;
        private bool _eventsSetup;
        private int? _position;
        public RtTimer Timer { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }
        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();

        public ObservableCollection<TimeDifference> SkierTimeData { get; set; } =
            new ObservableCollection<TimeDifference>();

        public ObservableCollection<RaceRanking> Ranking { get; set; } = new ObservableCollection<RaceRanking>();

        public ICommand ReadyTrackCommand { get; set; }
        public ICommand CancelSkierCommand { get; set; }
        public ICommand DisqualifyCurrentSkierCommand { get; set; }
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

        public ActiveRaceControlViewModel(SharedRaceStateViewModel raceState, IRaceService logic,
            IActiveRaceControlService activeRaceControlService)
        {
            RaceState = raceState;
            _logic = logic;
            _activeRaceControlService = activeRaceControlService;
            Timer = new RtTimer();
            SetupCommands();
        }

        private void SetupCommands()
        {
            ReadyTrackCommand = new AsyncCommand(async _ =>
                                                 {
                                                     try
                                                     {
                                                         await _activeRaceControlService.EnableRaceForSkier();
                                                         await LoadSplitTimes();
                                                     }
                                                     catch (Exception)
                                                     {
                                                         // ignored
                                                     }
                                                 },
                                                 _ =>
                                                     StartList.Any());
            CancelSkierCommand = new AsyncCommand(async skierId => await CancelSkier((int) skierId));
            CancelRaceCommand = new AsyncCommand(async _ => await CancelRace());
            DisqualifyCurrentSkierCommand = new AsyncCommand(async _ =>
                                                             {
                                                                 Timer.Reset();
                                                                 await _activeRaceControlService
                                                                     .DisqualifyCurrentSkier();
                                                             });
        }

        public async Task SetupAsync()
        {
            if (!_eventsSetup) SetupRaceEvents();
            await LoadData();
            if (CurrentSkier != null) Timer.Start();
        }

        private async Task LoadData()
        {
            try
            {
                await LoadCurrentSkier();
                await LoadStartList();
                await LoadSplitTimes();
                await LoadRanking();
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
                SkierTimeData
                    .Repopulate(
                        await _logic.GetTimeDataForSkierWithDifference(CurrentSkier.SkierId, RaceState.Race.Id));
            }
            else SkierTimeData.Clear();

            Timer.StartTime = startTime;
        }

        private async Task LoadRanking() =>
            Ranking.Repopulate(await _logic.GetRankingForRace(RaceState.Race.Id));

        private void SetupRaceEvents()
        {
            static Task UpdateInUiThread(Func<Task> func) =>
                Application.Current.Dispatcher?.Invoke(async () => await func());

            _activeRaceControlService.OnSkierStarted += async startList =>
            {
                CurrentSkier = startList;
                try
                {
                    await LoadStartList();
                    Timer.Start();
                }
                catch (Exception)
                {
                    ErrorNotifier.OnLoadError();
                }
            };
            _activeRaceControlService.OnSplitTime += async timeData => await OnSplitTime(timeData);
            _activeRaceControlService.OnSkierCanceled += async _ => await UpdateInUiThread(LoadStartList);
            _activeRaceControlService.OnLateDisqualification += async _ => await UpdateInUiThread(LoadRanking);
            _activeRaceControlService.OnCurrentSkierDisqualified += async _ =>
            {
                await UpdateInUiThread(LoadRanking);
                Timer.Reset();
            };
            _activeRaceControlService.OnSkierFinished += async _ =>
            {
                Timer.Reset();
                await UpdateInUiThread(LoadRanking);
            };
            _eventsSetup = true;
        }

        private async Task OnSplitTime(TimeData timeData)
        {
            try
            {
                if (timeData.Sensor.SensorNumber != 0)
                    Position = await _activeRaceControlService.GetPossiblePositionForCurrentSkier();

                Timer.StartTime ??= timeData.SkierEvent.RaceData.EventDateTime;
                var difference = await _logic.GetDifferenceToLeader(timeData);
                if (difference == null) return;

                Application.Current.Dispatcher?.Invoke(() => SkierTimeData.Add(new TimeDifference
                {
                    TimeData = timeData,
                    DifferenceToLeader = difference.Value.Milliseconds
                }));
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