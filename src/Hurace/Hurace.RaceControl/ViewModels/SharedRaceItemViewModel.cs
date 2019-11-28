using System.Collections.ObjectModel;
using Hurace.Core.Dto;

namespace Hurace.RaceControl.ViewModels
{
    public class SharedRaceItemViewModel
    {
        public ObservableCollection<Gender> Genders { get; set; } = new ObservableCollection<Gender>();
        public ObservableCollection<Discipline> Disciplines { get; set; } = new ObservableCollection<Discipline>();
        public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();
    }
}