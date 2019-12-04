using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api.RaceControl;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.Util;
using Microsoft.Xaml.Behaviors.Core;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceControlViewModel : NotifyPropertyChanged
    {
        public Race Race
        {
            get => _race;
            set => Set(ref _race, value);
        }

        public ICommand StartRaceCommand { get; set; }
        private IRaceControlService _raceControlService;
        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();
        private Race _race;

        public RaceControlViewModel(Race race, IRaceControlService raceControlService)
        {
            Race = race;
            _raceControlService = raceControlService;
            StartRaceCommand = new ActionCommand(_ =>
            {
                if (MessageBox.Show("Rennen kann nach dem Starten nicht mehr bearbeitet werden. Fortfahren ?",
                                    "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                    MessageBoxResult.Yes) return;
                _raceControlService.StartRace(Race);
                InvokePropertyChanged(nameof(Race));
            });
        }

        public async Task SetupAsync()
        {
            var startList = await _raceControlService.GetStartListForRace(Race.Id);
            StartList.Clear();
            StartList.AddRange(startList);
        }
    }
}