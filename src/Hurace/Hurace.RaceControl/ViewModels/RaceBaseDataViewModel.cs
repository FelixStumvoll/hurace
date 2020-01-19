using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.Validators;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceBaseDataViewModel : ValidatorBase<RaceBaseDataViewModel, RaceValidator>
    {
        private readonly IRaceService _raceService;
        private readonly ILocationService _locationService;
        private int _sensorCount;
        private Discipline _selectedDiscipline;
        public event Action OnUnsavedCancel;
        public SharedRaceViewModel SharedRaceViewModel { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }
        public ObservableCollection<Discipline> Disciplines { get; set; } = new ObservableCollection<Discipline>();
        public ICommand StartEditCommand { get; set; }
        public ICommand SaveEditCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public ICommand LocationChangedCommand { get; set; }

        public Discipline SelectedDiscipline
        {
            get => _selectedDiscipline;
            set => Set(ref _selectedDiscipline, value);
        }

        public Gender SelectedGender
        {
            get => RaceState.Race.Gender;
            set
            {
                RaceState.Race.Gender = value;
                RaceState.Race.GenderId = value?.Id ?? -1;
                InvokePropertyChanged(nameof(SelectedGender));
            }
        }

        public Location SelectedLocation
        {
            get => RaceState.Race.Location;
            set
            {
                RaceState.Race.Location = value;
                RaceState.Race.LocationId = value?.Id ?? -1;
                InvokePropertyChanged(nameof(SelectedLocation));
            }
        }

        public int SensorCount
        {
            get => _sensorCount;
            set => Set(ref _sensorCount, value);
        }

        public Season Season => RaceState.Race.Season;

        public RaceBaseDataViewModel(SharedRaceStateViewModel raceState, SharedRaceViewModel svm,
            IRaceService raceService, ILocationService locationService)
        {
            RaceState = raceState;
            _raceService = raceService;
            _locationService = locationService;
            SharedRaceViewModel = svm;
            SetupCommands();
            RegisterValidator(this);
        }

        private void SetupCommands()
        {
            StartEditCommand = new RelayCommand(StartEdit, () => RaceState.Race.RaceStateId ==
                                                                 (int) Dal.Domain.Enums.RaceState.Upcoming);
            CancelEditCommand = new AsyncCommand(CancelEdit);
            SaveEditCommand = new AsyncCommand(SaveEdit, () => ValidatorIsValid);
            LocationChangedCommand = new AsyncCommand(SetDisciplinesForLocation);
        }

        public async Task SetupAsync()
        {
            try
            {
                ValidatorEnabled = false;
                var sensorCount = await _raceService.GetSensorCount(RaceState.Race.Id);
                SensorCount = sensorCount.Value;
                await SetSelectedProps();
                ValidatorEnabled = true;
            }
            catch (Exception)
            {
                MessageBoxUtil.Error("Fehler beim Laden der Renndaten");
            }
        }

        private async Task SetSelectedProps()
        {
            SelectedGender = SharedRaceViewModel.Genders.DataSource
                                                .SingleOrDefault(g => g.Id == RaceState.Race.GenderId);
            SelectedLocation = SharedRaceViewModel.Locations.DataSource
                                                  .SingleOrDefault(l => l.Id == RaceState.Race.LocationId);
            await SetDisciplinesForLocation();
        }

        private async Task SetDisciplinesForLocation()
        {
            if (SelectedLocation == null) return;
            var disciplines = await _locationService.GetDisciplinesForLocation(SelectedLocation.Id);
            Disciplines.Clear();
            Disciplines.AddRange(disciplines);
            SelectedDiscipline = Disciplines.SingleOrDefault(d => d.Id == RaceState.Race.DisciplineId);
        }

        private void StartEdit() => RaceState.Edit = true;

        private async Task CancelEdit()
        {
            RaceState.Edit = false;
            if (RaceState.Race.Id == -1)
            {
                OnUnsavedCancel?.Invoke();
                return;
            }

            await UpdateRace();
            await SetSelectedProps();
        }

        private async Task SaveEdit()
        {
            RaceState.Race.DisciplineId = _selectedDiscipline.Id;
            RaceState.Race.Discipline = _selectedDiscipline;
            switch (await _raceService.InsertOrUpdateRace(RaceState.Race, SensorCount))
            {
                case RaceModificationResult.Ok:
                    RaceState.Edit = false;
                    await UpdateRace();
                    break;
                case RaceModificationResult.Err:
                    MessageBoxUtil.Error("Fehler beim Speichern des Rennens");
                    break;
                case RaceModificationResult.StartListDefined:
                   MessageBoxUtil.Error("Geschlecht und Disziplin können nicht geändert werden, " +
                                       "wenn eine Startliste definiert ist");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task UpdateRace()
        {
            try
            {
                RaceState.Race = await _raceService.GetRaceById(RaceState.Race.Id);
            }
            catch (Exception)
            {
                MessageBoxUtil.Error("Rennen konnte nicht geladen werden");
            }
        }
    }
}