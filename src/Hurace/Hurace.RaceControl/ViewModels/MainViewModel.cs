using System;
using System.Collections.ObjectModel;
using Hurace.Core.Dto;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<RaceItemViewModel> Races { get; set; } = new ObservableCollection<RaceItemViewModel>();

        public RaceItemViewModel CurrentRace { get; set; }

        public MainViewModel()
        {
            Races.Add(new RaceItemViewModel(new Race
            {
                Id = 1,
                RaceDescription = "Yeet",
                Discipline = new Discipline{DisciplineName = "Abfahrt"},
                Gender = new Gender{GenderDescription = "Herren"},
                Location =  new Location{LocationName = "Kitz", Country = new Country()},
                Season = new Season(),
                RaceDate = DateTime.Now,
                RaceState = new RaceState{RaceStateDescription = "Lul"}
            }));

            

        }
    }
}