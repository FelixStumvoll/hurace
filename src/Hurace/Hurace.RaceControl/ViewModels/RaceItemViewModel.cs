using System.Windows.Input;
using DeepCopy;
using Hurace.Core.Dto;
using Hurace.RaceControl.ViewModels.Commands;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceItemViewModel : NotifyPropertyChanged
    {
        private bool _edit;
        private Race? _backupRace;
        private Race _race;

        public RaceItemViewModel(Race race)
        {
            Race = race;
            StartEdit = new ActionCommand(_ =>
            {
                _backupRace = DeepCopier.Copy(Race);
                Edit = true;
            });

            SaveEdit = new ActionCommand(_ =>
            {
                _backupRace = null;
                Edit = false;
            });
            
            CancelEdit = new ActionCommand(_ =>
            {
                Race = DeepCopier.Copy(_backupRace);
                _backupRace = null;
                Edit = false;
            });
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

        public ICommand StartEdit { get; set; }
        public ICommand SaveEdit { get; set; }
        public ICommand CancelEdit { get; set; }
    }
}