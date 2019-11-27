using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DeepCopy;
using Hurace.Core.Api;
using Hurace.Core.Dto;
using Hurace.RaceControl.ViewModels.Commands;
using Microsoft.VisualBasic;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceItemViewModel : NotifyPropertyChanged
    {
        private bool _edit;
        private Race? _backupRace;
        private Race _race;
        public RaceItemStartListViewModel RaceItemStartListViewModel { get; set; }

        public ICommand StartEdit { get; set; }
        public ICommand Delete { get; set; }
        public ICommand SaveEdit { get; set; }
        public ICommand CancelEdit { get; set; }
        public ObservableCollection<Discipline> Disciplines { get; set; } = new ObservableCollection<Discipline>();
        public ObservableCollection<Gender> Genders { get; set; } = new ObservableCollection<Gender>();

        public Discipline SelectedDiscipline
        {
            get => Race.Discipline;
            set
            {
                Race.Discipline = value;
                Race.DisciplineId = value.Id;
                InvokePropertyChanged();
            }
        }

        public Gender SelectedGender
        {
            get => Race.Gender;
            set
            {
                Race.Gender = value;
                Race.GenderId = value.Id;
                InvokePropertyChanged();
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

        private IHuraceCore _logic;

        public RaceItemViewModel(IHuraceCore logic, Race race, Func<RaceItemViewModel, bool> deleteFunc)
        {
            _logic = logic;
            Race = race;
            _backupRace = new Race();
            RaceItemStartListViewModel = new RaceItemStartListViewModel(logic, Race);
            StartEdit = new ActionCommand(_ =>
            {
                ShallowCopy(Race, _backupRace);
                Edit = true;
            });

            SaveEdit = new ActionCommand(_ =>
            {
                Edit = false;
            });

            CancelEdit = new ActionCommand(_ =>
            {
                ShallowCopy(_backupRace, Race);
                Edit = false;
            });

            Delete = new ActionCommand(_ => { deleteFunc(this); });
            Disciplines.Add(new Discipline{DisciplineName = "Downhill"});
            Disciplines.Add(new Discipline{DisciplineName = "Super-G"});
        }

        private static void ShallowCopy(Race original, Race copyTarget)
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