using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.Models;
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
        private bool _setupDone;
        private bool _startListDefined;

        public SharedRaceStateViewModel RaceState { get; set; }
        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();

        public ObservableCollection<TimeDifference> SkierTimeData { get; set; } =
            new ObservableCollection<TimeDifference>();

        public ObservableCollection<RaceRanking> Ranking { get; set; } = new ObservableCollection<RaceRanking>();

        public ICommand StartRaceCommand { get; set; }
        public ICommand ReadyTrackCommand { get; set; }
        public ICommand CancelSkier { get; set; }

        public StartList CurrentSkier
        {
            get => _currentSkier;
            set => Set(ref _currentSkier, value);
        }

        public bool StartListDefined
        {
            get => _startListDefined;
            set => Set(ref _startListDefined, value);
        }

        public RaceControlViewModel(SharedRaceStateViewModel raceState, IRaceService logic)
        {
            RaceState = raceState;
            _logic = logic;
            SetupCommands();
        }

        private void SetupCommands()
        {
            StartRaceCommand = new AsyncCommand(_ => StartRace(), _ => StartListDefined);
            ReadyTrackCommand = new AsyncCommand(async _ => await _activeRaceControlService.EnableRaceForSkier(),
                                                 _ => CurrentSkier == null && StartList.Any());
            CancelSkier = new AsyncCommand(async skierId =>
                                               await _activeRaceControlService.CancelSkier((int) skierId));
        }

        public async Task SetupAsync()
        {
            _activeRaceControlService ??= ActiveRaceResolver.Instance[RaceState.Race.Id];
            StartListDefined = await _logic.IsStartListDefined(RaceState.Race.Id) ?? false;
            if (_activeRaceControlService == null) return;
            if (!_setupDone) SetupRaceEvents();
            await LoadData();
            _setupDone = true;
        }

        private async Task LoadData()
        {
            try
            {
                //current skier
                CurrentSkier = await _activeRaceControlService.GetCurrentSkier();

                //startlist
                StartList.Repopulate(await _activeRaceControlService.GetRemainingStartList());
                InvokePropertyChanged(nameof(StartListDefined));

                //timedata for current
                if (CurrentSkier != null)
                    SkierTimeData
                        .Repopulate(
                            await _logic.GetTimeDataForSkierWithDifference(CurrentSkier.SkierId, RaceState.Race.Id));


                Ranking.Repopulate(await _logic.GetRankingForRace(RaceState.Race.Id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                ErrorNotifier.OnLoadError();
            }
        }

        private void SetupRaceEvents()
        {
            _activeRaceControlService.OnSkierStarted += async startList =>
            {
                CurrentSkier = startList;
                await LoadStartList();
            };
            _activeRaceControlService.OnSplitTime += async timeData =>
            {
                try
                {
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
            };

            Task LoadDataInViewThread() => Application.Current.Dispatcher?.Invoke(async () => await LoadData());

            _activeRaceControlService.OnSkierCanceled += async _ => await LoadDataInViewThread();
            _activeRaceControlService.OnLateDisqualification += async _ => await LoadDataInViewThread();
            _activeRaceControlService.OnCurrentSkierDisqualified += async _ => await LoadDataInViewThread();
            _activeRaceControlService.OnSkierFinished += async _ => await LoadDataInViewThread();
        }

        private async Task StartRace()
        {
            if (MessageBox.Show("Rennen kann nach dem Starten nicht mehr bearbeitet werden. Fortfahren ?",
                                "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;

            try
            {
                _activeRaceControlService = await ActiveRaceResolver.Instance.StartRace(RaceState.Race.Id);
                RaceState.Race = await _logic.GetRaceById(RaceState.Race.Id);
                await SetupAsync();
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }

        private async Task LoadStartList()
        {
            try
            {
                StartList.Repopulate(await _activeRaceControlService.GetRemainingStartList());
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }
    }
}