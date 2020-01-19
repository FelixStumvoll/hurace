using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Interface;
using Hurace.Core.Interface.Configs;
// using Hurace.Core.Service.Services.ActiveRaceControlService.Resolver;
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
        private readonly IRaceService _raceService;
        private readonly IRaceStartListService _startListService;
        private IRaceControlViewModel _raceControlViewModel;
        private ActiveRaceControlViewModel _activeRaceControlViewModel;
        private readonly SensorConfig _sensorConfig;

        private readonly Func<SharedRaceStateViewModel, IActiveRaceControlService, ActiveRaceControlViewModel>
            _activeRaceControlVmFactory;

        private readonly Func<SharedRaceStateViewModel, ReadonlyRaceControlViewModel>
            _readonlyRaceControlVmFactory;

        public SharedRaceStateViewModel RaceState { get; set; }
        private ReadonlyRaceControlViewModel _readonlyRaceControlViewModel;
        private bool _sensorMismatch;
        public ICommand StartRaceCommand { get; set; }

        public bool StartListDefined
        {
            get => _startListDefined;
            set => Set(ref _startListDefined, value);
        }

        public bool SensorMismatch
        {
            get => _sensorMismatch;
            set => Set(ref _sensorMismatch, value);
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
            SharedRaceStateViewModel raceState, IRaceService raceService,
            IRaceStartListService startListService, IActiveRaceResolver activeRaceResolver, SensorConfig sensorConfig)
        {
            RaceState = raceState;
            _raceService = raceService;
            _startListService = startListService;
            _activeRaceResolver = activeRaceResolver;
            _sensorConfig = sensorConfig;

            _activeRaceControlVmFactory = activeRaceControlVmFactory;
            _readonlyRaceControlVmFactory = readonlyRaceControlVmFactory;
            StartRaceCommand = new AsyncCommand(StartRace, () => StartListDefined);
        }

        public async Task SetupAsync()
        {
            _activeRaceControlService ??= _activeRaceResolver[RaceState.Race.Id];

            try
            {
                SensorMismatch = _sensorConfig.SensorAssumptions.Count !=
                                  await _raceService.GetSensorCount(RaceState.Race.Id);
            }
            catch (Exception)
            {
                // ignored
            }

            switch (RaceState.Race.RaceStateId)
            {
                case (int) Dal.Domain.Enums.RaceState.Upcoming:
                    StartListDefined = await _startListService.IsStartListDefined(RaceState.Race.Id) ?? false;
                    await SetRaceControlViewModel(ViewType.None);
                    return;
                case (int) Dal.Domain.Enums.RaceState.Running:
                    await SetRaceControlViewModel(ViewType.Active);
                    SetupRaceEndEvents();

                    return;
                case (int) Dal.Domain.Enums.RaceState.Finished:
                case (int) Dal.Domain.Enums.RaceState.Cancelled:
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

        private void SetupRaceEndEvents()
        {
            async Task RaceEnd(Race race)
            {
                UiExecutor.ExecuteInUiThread(() => RaceState.Race = race);
                await SetupAsync();
            }

            _activeRaceControlService.OnRaceCancelled += async race => await RaceEnd(race);
            _activeRaceControlService.OnRaceFinished += async race => await RaceEnd(race);
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
                RaceState.Race = await _raceService.GetRaceById(RaceState.Race.Id);
                SetupRaceEndEvents();
                await SetRaceControlViewModel(ViewType.Active);
            }
            catch (Exception)
            {
                MessageBoxUtil.Error("Rennen konnte nicht gestartet werden");
            }
        }
    }
}