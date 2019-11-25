using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Dto;
using Hurace.RaceControl.ViewModels.Commands;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private RaceItemViewModel _currentRace;

        public ObservableCollection<RaceItemViewModel> Races { get; set; } =
            new ObservableCollection<RaceItemViewModel>();

        public RaceItemViewModel CurrentRace
        {
            get => _currentRace;
            set => Set(ref _currentRace, value);
        }

        public ICommand Delete { get; set; }

        public MainViewModel()
        {
            Races.Add(new RaceItemViewModel(new Race
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

            Races.Add(new RaceItemViewModel(new Race
            {
                Id = 1,
                RaceDescription = "Despacito Bottomtext",
                Discipline = new Discipline {DisciplineName = "Abfahrt"},
                Gender = new Gender {GenderDescription = "Herren"},
                Location = new Location {LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState {RaceStateDescription = "Lul"}
            },DeleteRace));

            Delete = new ActionCommand(_ => DeleteRace(CurrentRace), _ => CurrentRace != null);
        }

        private bool DeleteRace(RaceItemViewModel rvm)
        {
            if (MessageBox.Show("Delete Race ?", "Delete ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return false;
            Races.Remove(CurrentRace);
            CurrentRace = null;
            return true;
        }
    }
}