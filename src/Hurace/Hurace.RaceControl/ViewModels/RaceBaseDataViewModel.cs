using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hurace.Core.Logic.RaceBaseDataService;
using Hurace.Core.Logic.Util;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.Validators;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceBaseDataViewModel : ValidatorViewModel<RaceBaseDataViewModel, RaceValidator>
    {
        private readonly IRaceBaseDataService _baseDataService;
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

        public RaceBaseDataViewModel(SharedRaceViewModel svm, SharedRaceStateViewModel raceState, IRaceBaseDataService baseDataService)
        {
            RaceState = raceState;
            _baseDataService = baseDataService;
            SharedRaceViewModel = svm;
            SetupCommands();
            RegisterValidator(this);
        }

        private void SetupCommands()
        {
            StartEditCommand = new RelayCommand(StartEdit);
            CancelEditCommand = new AsyncCommand(CancelEdit);
            SaveEditCommand = new AsyncCommand(SaveEdit, () => ValidatorIsValid);
            LocationChangedCommand = new AsyncCommand(SetDisciplinesForLocation);
        }

        public async Task SetupAsync()
        {
            ValidatorEnabled = false;
            var sensorCount = await _baseDataService.GetSensorCount(RaceState.Race.Id);
            if (sensorCount == null)
            {
                ErrorNotifier.OnLoadError();
                return;
            }

            SensorCount = sensorCount.Value;
            await SetSelectedProps();
            ValidatorEnabled = true;
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
            try
            {
                var disciplines = await _baseDataService.GetDisciplinesForLocation(SelectedLocation.Id);
                Disciplines.Clear();
                Disciplines.AddRange(disciplines);
                SelectedDiscipline = Disciplines.SingleOrDefault(d => d.Id == RaceState.Race.DisciplineId);
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
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
            switch (await _baseDataService.InsertOrUpdateRace(RaceState.Race, SensorCount))
            {
                case RaceModificationResult.Ok:
                    RaceState.Edit = false;
                    await UpdateRace();
                    break;
                case RaceModificationResult.Err:
                    MessageBox.Show("Fehler beim Speichern des Rennens",
                                    "Fehler",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    break;
                case RaceModificationResult.StartListDefined:
                    MessageBox.Show("Geschlecht und Disziplin können nicht geändert werden, " +
                                    "wenn eine Startliste definiert ist",
                                    "Fehler",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task UpdateRace()
        {
            try
            {
                RaceState.Race = await _baseDataService.GetRaceById(RaceState.Race.Id);
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }
    }
}