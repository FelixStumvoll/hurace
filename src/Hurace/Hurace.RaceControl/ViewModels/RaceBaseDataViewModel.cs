using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api.RaceCrud;
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
        public event Action OnUnsavedCancel;
        public ICommand StartEditCommand { get; set; }
        public ICommand SaveEditCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public SharedRaceViewModel SharedRaceViewModel { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }
        public Discipline SelectedDiscipline
        {
            get => RaceState.Race.Discipline;
            set
            {
                RaceState.Race.Discipline = value;
                RaceState.Race.DisciplineId = value?.Id ?? -1;
                InvokePropertyChanged(nameof(SelectedDiscipline));
            }
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
        }

        public async Task SetupAsync()
        {
            SensorCount = await _logic.GetSensorCount(RaceState.Race.Id);
            SetSelectedProps();
        }

        private void SetSelectedProps()
        {
            SelectedDiscipline =
                SharedRaceViewModel.Disciplines.DataSource.SingleOrDefault(d => d.Id == RaceState.Race.DisciplineId);
            SelectedGender = SharedRaceViewModel.Genders.DataSource.SingleOrDefault(g => g.Id == RaceState.Race.GenderId);
            SelectedLocation = SharedRaceViewModel.Locations.DataSource.SingleOrDefault(l => l.Id == RaceState.Race.LocationId);
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

            RaceState.Race = await _logic.GetRaceById(RaceState.Race.Id);
            
            SetSelectedProps();
            InvokePropertyChanged(nameof(Race));
        }

        private async Task SaveEdit(object _)
        {
            if (await _logic.InsertOrUpdateRace(RaceState.Race, SensorCount)) RaceState.Edit = false;
        }

        private bool SaveValidator(object _) =>
            RaceState.Race.LocationId != -1 && RaceState.Race.GenderId != -1 && RaceState.Race.DisciplineId != -1 &&
            !RaceState.Race.RaceDescription.IsNullOrEmpty() && RaceState.Race.RaceDate != DateTime.MinValue && SensorCount > 0;
    }
}