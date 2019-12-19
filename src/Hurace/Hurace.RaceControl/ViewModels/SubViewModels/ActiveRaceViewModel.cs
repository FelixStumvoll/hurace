using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.BaseViewModels;

namespace Hurace.RaceControl.ViewModels.SubViewModels
{
    public class ActiveRaceViewModel : NotifyPropertyChanged
    {
        private StartList _currentSkier;

        public StartList CurrentSkier
        {
            get => _currentSkier;
            set => Set(ref _currentSkier, value);
        }
    }
}