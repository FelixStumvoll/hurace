using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.SharedViewModels
{
    public class SharedRaceViewModel
    {
        public FilterableObservableCollection<Gender> Genders { get; set; }
        public FilterableObservableCollection<Discipline> Disciplines { get; set; } //todo remove maybe
        public FilterableObservableCollection<Location> Locations { get; set; }

        public SharedRaceViewModel()
        {
            Genders = new FilterableObservableCollection<Gender>((g, s) =>
                                                                     g.GenderDescription.ToLower().Contains(s));
            Disciplines =
                new FilterableObservableCollection<Discipline>((d, s) =>
                                                                   d.DisciplineName.ToLower().Contains(s));
            Locations = new FilterableObservableCollection<Location>((l, s) =>
                                                                         l.LocationName.ToLower().Contains(s));
        }
    }
}