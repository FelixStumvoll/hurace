using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver;
// using Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Service;
using Hurace.Core.Logic.Services.RaceBaseDataService;
using Hurace.Core.Logic.Services.RaceStartListService;
using Hurace.Dal.Domain;
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
        private readonly IActiveRaceResolver _activeRaceResolver;
        private readonly IRaceBaseDataService _baseDataService;
        private readonly IRaceStartListService _startListService;
        private IRaceControlViewModel _raceControlViewModel;
        private ActiveRaceControlViewModel _activeRaceControlViewModel;

        private readonly Func<SharedRaceStateViewModel, IActiveRaceControlService, ActiveRaceControlViewModel>
            _activeRaceControlVmFactory;
        
        private readonly Func<SharedRaceStateViewModel, ReadonlyRaceControlViewModel>
            _readonlyRaceControlVmFactory;

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

        public RaceControlBaseViewModel(
            Func<SharedRaceStateViewModel, IActiveRaceControlService, ActiveRaceControlViewModel>
                activeRaceControlVmFactory,
            Func<SharedRaceStateViewModel, ReadonlyRaceControlViewModel> readonlyRaceControlVmFactory,
            SharedRaceStateViewModel raceState, IRaceBaseDataService baseDataService,
            IRaceStartListService startListService, IActiveRaceResolver activeRaceResolver)
        {
            RaceState = raceState;
            _baseDataService = baseDataService;
            _startListService = startListService;
            _activeRaceResolver = activeRaceResolver;

            _activeRaceControlVmFactory = activeRaceControlVmFactory;
            _readonlyRaceControlVmFactory = readonlyRaceControlVmFactory;
            StartRaceCommand = new AsyncCommand(StartRace, () => StartListDefined);
        }

        public async Task SetupAsync()
        {
            _activeRaceControlService ??= _activeRaceResolver[RaceState.Race.Id];
            switch (RaceState.Race.RaceStateId)
            {
                case (int) Dal.Domain.Enums.RaceState.Upcoming:
                    StartListDefined = await _startListService.IsStartListDefined(RaceState.Race.Id) ?? false;
                    await SetRaceControlViewModel(ViewType.None);
                    return;
                case (int) Dal.Domain.Enums.RaceState.Running:
                    await SetRaceControlViewModel(ViewType.Active);
                    return;
                case (int) Dal.Domain.Enums.RaceState.Finished:
                    await SetRaceControlViewModel(ViewType.Readonly);
                    return;
            }
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
                    _activeRaceControlVmFactory(RaceState, _activeRaceControlService),
                ViewType.Readonly => _readonlyRaceControlViewModel ??= _readonlyRaceControlVmFactory(RaceState),
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
                _activeRaceControlService = await _activeRaceResolver.StartRace(RaceState.Race.Id);
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