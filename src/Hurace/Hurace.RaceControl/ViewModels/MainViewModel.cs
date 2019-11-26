using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Dto;
using Hurace.RaceControl.ViewModels.Commands;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private RaceItemViewModel _currentRace;

        private readonly IHuraceCore _logic;

        public ObservableCollection<RaceItemViewModel> Races { get; set; } =
            new ObservableCollection<RaceItemViewModel>();

        public ObservableCollection<RaceItemViewModel> ActiveRaces { get; set; } =
            new ObservableCollection<RaceItemViewModel>();

        public RaceItemViewModel CurrentRace
        {
            get => _currentRace;
            set => Set(ref _currentRace, value);
        }

        public ICommand AddRace { get; set; }

        public MainViewModel(IHuraceCore logic)
        {
            _logic = logic;
            AddRace = new ActionCommand(_ =>
            {
                var rvm = new RaceItemViewModel(_logic, new Race(), DeleteRace) {Edit = true};
                Races.Add(rvm);
                CurrentRace = rvm;
            });
        }

        private bool DeleteRace(RaceItemViewModel rvm)
        {
            if (MessageBox.Show("Delete Race ?", "Delete ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return false;
            Races.Remove(CurrentRace);
            ActiveRaces.Remove(CurrentRace);
            CurrentRace = null;
            return true;
        }

        public async Task InitializeAsync()
        {
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            Races.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));

            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
            ActiveRaces.Add(new RaceItemViewModel(_logic, new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            }, DeleteRace));
        }
    }
}