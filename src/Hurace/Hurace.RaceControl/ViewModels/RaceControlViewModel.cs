using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api.RaceControl;
using Hurace.Core.Api.RaceCrud;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;
using ActionCommand = Microsoft.Xaml.Behaviors.Core.ActionCommand;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceControlViewModel : NotifyPropertyChanged
    {
        private IRaceControlService _raceControlService;
        private Race _race;
        private StartList _currentSkier;
        private readonly IRaceService _logic;
        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();
        public ICommand StartRaceCommand { get; set; }
        public ICommand ReadyTrackCommand { get; set; }
        
        public Race Race
        {
            get => _race;
            set => Set(ref _race, value);
        }

        public StartList CurrentSkier
        {
            get => _currentSkier;
            set => Set(ref _currentSkier, value);
        }

        public RaceControlViewModel(Race race, IRaceService logic)
        {
            Race = race; 
            _logic = logic;
            SetupCommands();
        }

        private void SetupCommands()
        {
            StartRaceCommand = new AsyncCommand(StartRace);
            ReadyTrackCommand = new ActionCommand(_ =>
            {
                _raceControlService.EnableRaceForSkier(Race);
                CurrentSkier = CurrentSkier == null ? new StartList() : null;
            });
        }

        public async Task SetupAsync()
        {
            var startList = await _logic.GetStartListForRace(Race.Id);
            StartList.Clear();
            StartList.AddRange(startList);
        }

        private async Task StartRace(object _)
        {
            if (MessageBox.Show("Rennen kann nach dem Starten nicht mehr bearbeitet werden. Fortfahren ?",
                                "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;
            _raceControlService = await ActiveRaceHandler.Instance.StartRace(Race.Id);
            InvokePropertyChanged(nameof(Race));
        }
    }
}