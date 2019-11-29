using System.Collections.ObjectModel;
using Hurace.Dal.Domain;

namespace Hurace.RaceControl.ViewModels
{
    public class SharedRaceViewModel
    {
        public ObservableCollection<Gender> Genders { get; set; } = new ObservableCollection<Gender>();
        public ObservableCollection<Discipline> Disciplines { get; set; } = new ObservableCollection<Discipline>();
        public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();
    }
}