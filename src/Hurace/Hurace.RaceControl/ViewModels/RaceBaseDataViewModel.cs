using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceBaseDataViewModel : NotifyPropertyChanged
    {
        private readonly IRaceService _logic;
        private int _sensorCount = 1;
        private Discipline _selectedDiscipline;
        public event Action OnUnsavedCancel;
        public ICommand StartEditCommand { get; set; }
        public ICommand SaveEditCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public ICommand LocationChangedCommand { get; set; }
        public SharedRaceViewModel SharedRaceViewModel { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }
        public ObservableCollection<Discipline> Disciplines { get; set; } = new ObservableCollection<Discipline>();

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

        public RaceBaseDataViewModel(IRaceService logic, SharedRaceViewModel svm, SharedRaceStateViewModel raceState)
        {
            _logic = logic;
            RaceState = raceState;
            SharedRaceViewModel = svm;
            SetupCommands();
        }

        private void SetupCommands()
        {
            StartEditCommand = new ActionCommand(StartEdit);
            CancelEditCommand = new AsyncCommand(CancelEdit);
            SaveEditCommand = new AsyncCommand(SaveEdit, SaveValidator);
            LocationChangedCommand = new AsyncCommand(_ => LoadDisciplinesForLocation());
        }

        public async Task SetupAsync()
        {
            SensorCount = await _logic.GetSensorCount(RaceState.Race.Id);
            if (SensorCount == -1)
            {
                ErrorNotifier.OnLoadError();
                return;
            }
            await SetSelectedProps();
        }

        private async Task SetSelectedProps()
        {
            SelectedGender =
                SharedRaceViewModel.Genders.DataSource.SingleOrDefault(g => g.Id == RaceState.Race.GenderId);
            SelectedLocation =
                SharedRaceViewModel.Locations.DataSource.SingleOrDefault(l => l.Id == RaceState.Race.LocationId);
            await LoadDisciplinesForLocation();
        }

        private void StartEdit(object param) => RaceState.Edit = true;

        private async Task CancelEdit(object _)
        {
            RaceState.Edit = false;
            if (RaceState.Race.Id == -1)
            {
                OnUnsavedCancel?.Invoke();
                return;
            }

            if (!await UpdateRace()) return;
            await SetSelectedProps();
        }

        private async Task<bool> UpdateRace()
        {
            RaceState.Race = await _logic.GetRaceById(RaceState.Race.Id);
            if (RaceState.Race != null) return true;
            ErrorNotifier.OnLoadError();
            return false;
        }

        private async Task SaveEdit(object _)
        {
            RaceState.Race.DisciplineId = _selectedDiscipline.Id;
            RaceState.Race.Discipline = _selectedDiscipline;
            switch (await _logic.InsertOrUpdateRace(RaceState.Race, SensorCount))
            {
                case RaceUpdateState.Ok:
                    RaceState.Edit = false;
                    await UpdateRace();
                    break;
                case RaceUpdateState.Err:
                    MessageBox.Show("Fehler beim Speichern des Rennens", 
                                    "Fehler", 
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    break;
                case RaceUpdateState.StartListDefined:
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

        private async Task LoadDisciplinesForLocation()
        {
            if (SelectedLocation == null) return;
            var disciplines = await _logic.GetDisciplinesForLocation(SelectedLocation.Id);
            if (disciplines == null)
            {
                ErrorNotifier.OnLoadError();
                return;
            }
            Disciplines.Clear();
            Disciplines.AddRange(disciplines);
            SelectedDiscipline = Disciplines.SingleOrDefault(d => d.Id == RaceState.Race.DisciplineId);
        }

        private bool SaveValidator(object _) =>
            RaceState.Race.LocationId != -1 && RaceState.Race.GenderId != -1 &&
            (_selectedDiscipline != null && _selectedDiscipline.Id != -1) &&
            !RaceState.Race.RaceDescription.IsNullOrEmpty() &&
            RaceState.Race.RaceDate != DateTime.MinValue &&
            SensorCount > 0;
    }
}