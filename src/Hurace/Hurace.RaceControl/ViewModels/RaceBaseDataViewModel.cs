using System;
using System.Linq;
using System.Threading;
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
        private bool _edit;
        private Race _race;
        private readonly IRaceService _logic;
        private readonly Race _backupRace = new Race();
        private int _sensorCount;
        public event Action OnUnsavedCancel;
        public ICommand StartEditCommand { get; set; }
        public ICommand SaveEditCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public SharedRaceViewModel SharedRaceViewModel { get; set; }

        public Discipline SelectedDiscipline
        {
            get => Race.Discipline;
            set
            {
                Race.Discipline = value;
                Race.DisciplineId = value?.Id ?? -1;
                InvokePropertyChanged(nameof(SelectedDiscipline));
            }
        }

        public Gender SelectedGender
        {
            get => Race.Gender;
            set
            {
                Race.Gender = value;
                Race.GenderId = value?.Id ?? -1;
                InvokePropertyChanged(nameof(SelectedGender));
            }
        }

        public Location SelectedLocation
        {
            get => Race.Location;
            set
            {
                Race.Location = value;
                Race.LocationId = value?.Id ?? -1;
                InvokePropertyChanged(nameof(SelectedLocation));
            }
        }

        public int SensorCount
        {
            get => _sensorCount;
            set => Set(ref _sensorCount, value);
        }

        public Race Race
        {
            get => _race;
            set => Set(ref _race, value);
        }

        public bool Edit
        {
            get => _edit;
            set => Set(ref _edit, value);
        }

        public RaceBaseDataViewModel(IRaceService logic, Race race, SharedRaceViewModel svm)
        {
            _logic = logic;
            Race = race;
            SharedRaceViewModel = svm;

            Edit = race.Id == -1;
            
            StartEditCommand = new ActionCommand(StartEdit);
            CancelEditCommand = new ActionCommand(CancelEdit);
            SaveEditCommand = new AsyncCommand(SaveEdit, SaveValidator);
        }

        public async Task Setup()
        {
            SensorCount = await _logic.GetSensorCount(Race.Id);
            SetSelectedProps();
        }

        private void SetSelectedProps()
        {
            SelectedDiscipline =
                SharedRaceViewModel.Disciplines.DataSource.SingleOrDefault(d => d.Id == Race.DisciplineId);
            SelectedGender = SharedRaceViewModel.Genders.DataSource.SingleOrDefault(g => g.Id == Race.GenderId);
            SelectedLocation = SharedRaceViewModel.Locations.DataSource.SingleOrDefault(l => l.Id == Race.LocationId);
        }

        private void StartEdit(object param)
        {
            ShallowCopyRace(Race, _backupRace);
            Edit = true;
        }

        private void CancelEdit(object _)
        {
            Edit = false;
            if (Race.Id == -1)
            {
                OnUnsavedCancel?.Invoke();
                return;
            }
            
            ShallowCopyRace(_backupRace, Race);
            SetSelectedProps();
            InvokePropertyChanged(nameof(Race));
            
        }

        private async Task SaveEdit(object _)
        {
            if (await _logic.InsertOrUpdateRace(Race, SensorCount))
            {
                Edit = false;
            }
        }

        private bool SaveValidator(object _) =>
            Race.LocationId != -1 && Race.GenderId != -1 && Race.DisciplineId != -1 &&
            !Race.RaceDescription.IsNullOrEmpty() && Race.RaceDate != DateTime.MinValue && SensorCount > 0;

        private static void ShallowCopyRace(Race original, Race copyTarget)
        {
            copyTarget.DisciplineId = original.DisciplineId;
            copyTarget.GenderId = original.GenderId;
            copyTarget.LocationId = original.LocationId;
            copyTarget.RaceDescription = original.RaceDescription;
            copyTarget.SeasonId = original.SeasonId;
            copyTarget.RaceStateId = original.RaceStateId;
        }
    }
}