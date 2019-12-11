using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class SharedRaceViewModel
    {
        public FilterableObservableCollection<Gender> Genders { get; set; }
        public FilterableObservableCollection<Discipline> Disciplines { get; set; }
        public FilterableObservableCollection<Location> Locations { get; set; }

        public SharedRaceViewModel()
        {
            Genders = new FilterableObservableCollection<Gender>((g, s) => g.GenderDescription.ToLower().Contains(s));
            Disciplines =
                new FilterableObservableCollection<Discipline>((d, s) => d.DisciplineName.ToLower().Contains(s));
            Locations = new FilterableObservableCollection<Location>((l, s) => l.LocationName.ToLower().Contains(s));
        }
    }
}