using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Logic;
using Hurace.Core.Logic.ActiveRaceControlService.Resolver;
using Hurace.Core.Logic.ActiveRaceControlService.Service;
using Hurace.Core.Logic.RaceBaseDataService;
using Hurace.Core.Logic.RaceStartListService;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.RaceControlViewModels
{
    public class RaceControlBaseViewModel : NotifyPropertyChanged
    {
        private bool _startListDefined;
        private IActiveRaceControlService _activeRaceControlService;

        private readonly IRaceBaseDataService _baseDataService;
        private readonly IRaceStartListService _startListService;
        private IRaceControlViewModel _raceControlViewModel;
        private ActiveRaceControlViewModel _activeRaceControlViewModel;
        public SharedRaceStateViewModel RaceState { get; set; }
        private ReadonlyRaceControlViewModel _readonlyRaceControlViewModel;
        public ICommand StartRaceCommand { get; set; }

        public bool StartListDefined
        {
            get => _startListDefined;
            set => Set(ref _startListDefined, value);
        }

        public IRaceControlViewModel RaceControlViewModel
        {
            get => _raceControlViewModel;
            set => Set(ref _raceControlViewModel, value);
        }

        public RaceControlBaseViewModel(SharedRaceStateViewModel raceState, IRaceBaseDataService baseDataService,
            IRaceStartListService startListService)
        {
            RaceState = raceState;
            _baseDataService = baseDataService;
            _startListService = startListService;
            StartRaceCommand = new AsyncCommand(StartRace, () => StartListDefined);
        }

        public async Task SetupAsync()
        {
            _activeRaceControlService ??= ActiveRaceResolver.Instance[RaceState.Race.Id];
            if (_activeRaceControlService == null)
            {
                if (RaceState.Race.RaceStateId == (int) Dal.Domain.Enums.RaceState.Upcoming)
                {
                    StartListDefined = await _startListService.IsStartListDefined(RaceState.Race.Id) ?? false;
                    await SetRaceControlViewModel(ViewType.None);
                    return;
                }

                await SetRaceControlViewModel(ViewType.Readonly);
                return;
            }

            await SetRaceControlViewModel(ViewType.Active);
        }

        private enum ViewType
        {
            Active,
            Readonly,
            None
        }

        private async Task SetRaceControlViewModel(ViewType type)
        {
            RaceControlViewModel = type switch
            {
                ViewType.Active => _activeRaceControlViewModel ??=
                    new ActiveRaceControlViewModel(RaceState, _activeRaceControlService,
                                                   ServiceProvider.Instance.Resolve<IRaceStartListService>()),
                ViewType.Readonly => _readonlyRaceControlViewModel ??= new ReadonlyRaceControlViewModel(),
                ViewType.None => null,
                _ => null
            };

            if (RaceControlViewModel != null) await RaceControlViewModel.SetupAsync();
        }

        private async Task StartRace()
        {
            if (MessageBox.Show("Rennen kann nach dem Starten nicht mehr bearbeitet werden. Fortfahren ?",
                                "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;

            try
            {
                _activeRaceControlService = await ActiveRaceResolver.Instance.StartRace(RaceState.Race.Id);
                RaceState.Race = await _baseDataService.GetRaceById(RaceState.Race.Id);
                await SetRaceControlViewModel(ViewType.Active);
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }
    }
}