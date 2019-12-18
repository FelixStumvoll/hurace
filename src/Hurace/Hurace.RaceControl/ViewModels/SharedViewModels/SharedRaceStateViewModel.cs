using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.BaseViewModels;

namespace Hurace.RaceControl.ViewModels.SharedViewModels
{
    public class SharedRaceStateViewModel : NotifyPropertyChanged
    {
        private Race _race;
        private bool _edit;

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
    }
}