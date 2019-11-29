using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.Race;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceViewModel : NotifyPropertyChanged
    {
        private bool _edit;
        private Race _race;
        private readonly IRaceService _logic;
        private readonly Race _backupRace = new Race();
        private bool _isNew;
        public RaceStartListViewModel RaceStartListViewModel { get; set; }

        public ICommand StartEditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveEditCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public SharedRaceViewModel SharedRaceViewModel { get; set; }
        
        public Discipline SelectedDiscipline
        {
            get => Race.Discipline;
            set
            {
                Race.Discipline = value;
                Race.DisciplineId = value.Id;
                InvokePropertyChanged(nameof(SelectedDiscipline));
            }
        }

        public Gender SelectedGender
        {
            get => Race.Gender;
            set
            {
                Race.Gender = value;
                Race.GenderId = value.Id;
                InvokePropertyChanged(nameof(SelectedGender));
            }
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

        public bool IsNew
        {
            get => _isNew;
            set => Set(ref _isNew, value);
        }

        public RaceViewModel(IRaceService logic, Race race, SharedRaceViewModel svm,
            Func<RaceViewModel, bool> deleteFunc, bool isNew = false)
        {
            _logic = logic;
            Race = race;
            IsNew = isNew;
            SharedRaceViewModel = svm;
            RaceStartListViewModel = new RaceStartListViewModel(logic, Race);
            
            StartEditCommand = new ActionCommand(StartEdit);
            SaveEditCommand = new ActionCommand(SaveEdit);
            CancelEditCommand = new ActionCommand(CancelEdit);
            DeleteCommand = new ActionCommand(_ => deleteFunc(this));
        }

        public async Task SetupAsync()
        {
            SetComboboxData();
            await RaceStartListViewModel.SetupAsync();
        }

        private void SetComboboxData()
        {
            SelectedDiscipline = SharedRaceViewModel.Disciplines.SingleOrDefault(d => d.Id == Race.DisciplineId);
            SelectedGender = SharedRaceViewModel.Genders.SingleOrDefault(g => g.Id == Race.GenderId);
        }

        private void StartEdit(object param)
        {
            ShallowCopyRace(Race, _backupRace);
            Edit = true;
        }

        private void CancelEdit(object param)
        {
            ShallowCopyRace(_backupRace, Race);
            SetComboboxData();
            InvokePropertyChanged(nameof(Race));
            Edit = false;
        }

        private void SaveEdit(object param)
        {
            Edit = false;
        }

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