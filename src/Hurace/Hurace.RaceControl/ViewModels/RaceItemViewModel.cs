using System;
using System.Linq;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Dto;
using Hurace.RaceControl.ViewModels.Commands;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceItemViewModel : NotifyPropertyChanged
    {
        private bool _edit;
        private Race _race;
        private IHuraceCore _logic;
        public RaceItemStartListViewModel RaceItemStartListViewModel { get; set; }

        public ICommand StartEdit { get; set; }
        public ICommand Delete { get; set; }
        public ICommand SaveEdit { get; set; }
        public ICommand CancelEdit { get; set; }
        public SharedRaceItemViewModel SharedRaceItemViewModel { get; set; }


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

        public RaceItemViewModel(IHuraceCore logic, Race race, SharedRaceItemViewModel svm,
            Func<RaceItemViewModel, bool> deleteFunc)
        {
            _logic = logic;
            Race = race;
            SharedRaceItemViewModel = svm;
            var backupRace = new Race();
            RaceItemStartListViewModel = new RaceItemStartListViewModel(logic, Race);
            StartEdit = new ActionCommand(_ =>
            {
                ShallowCopyRace(Race, backupRace);
                Edit = true;
            });

            SaveEdit = new ActionCommand(_ => { Edit = false; });

            CancelEdit = new ActionCommand(_ =>
            {
                ShallowCopyRace(backupRace, Race);
                SelectedDiscipline =
                    SharedRaceItemViewModel.Disciplines.SingleOrDefault(d => d.Id == Race.DisciplineId);
                SelectedGender = SharedRaceItemViewModel.Genders.SingleOrDefault(g => g.Id == Race.GenderId);
                InvokePropertyChanged(nameof(Race));
                Edit = false;
            });

            Delete = new ActionCommand(_ => { deleteFunc(this); });
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