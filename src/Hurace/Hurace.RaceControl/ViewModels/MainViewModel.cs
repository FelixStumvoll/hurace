using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.RaceControl;
using Hurace.Core.Api.RaceCrud;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private RaceViewModel _selectedRace;
        private readonly IRaceControlService _raceControlService;

        private readonly IRaceService _logic;
        private readonly SharedRaceViewModel _sharedRaceViewModel;

        public ObservableCollection<RaceViewModel> Races { get; set; } =
            new ObservableCollection<RaceViewModel>();
        public RaceViewModel SelectedRace
        {
            get => _selectedRace;
            set => Set(ref _selectedRace, value);
        }
        
        public ICommand AddRaceCommand { get; set; }
        public ICommand SelectedRaceChangedCommand { get; set; }

        public MainViewModel(IRaceService logic, IRaceControlService raceControlService)
        {
            _raceControlService = raceControlService;
            _logic = logic;
            _sharedRaceViewModel = new SharedRaceViewModel();
            AddRaceCommand = new ActionCommand(_ =>
            {
                var rvm = new RaceViewModel(_logic, _raceControlService, new Race {Id = -1, RaceStateId = (int) Constants.RaceState.Upcoming, RaceDate = DateTime.Now}, _sharedRaceViewModel);
                rvm.OnDelete += async deleteRvm => await DeleteRace(deleteRvm);
                Races.Add(rvm);
                SelectedRace = rvm;
            });

            SelectedRaceChangedCommand = new ActionCommand(async _ =>
            {
                if (SelectedRace == null) return;
                await SelectedRace.SetupAsync();
            });
        }

        private async Task DeleteRace(RaceViewModel rvm)
        {
            if (MessageBox.Show("Rennen löschen ?", "Löschen ?", MessageBoxButton.YesNo, MessageBoxImage.Information) !=
                MessageBoxResult.Yes)
                return;
            if (rvm.Race.Id != -1)
            {
                if (!await _logic.RemoveRace(rvm.Race))
                    MessageBox.Show("Rennen konnte nicht gelöscht werden", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DeleteRaceViewModel(rvm);
        }

        private void DeleteRaceViewModel(RaceViewModel rvm)
        {
            Races.Remove(rvm);
            SelectedRace = null;
        }

        public async Task InitializeAsync()
        {
            foreach (var raceViewModel in (await _logic.GetAllRaces()).Select(
                r => new RaceViewModel(_logic,_raceControlService, r, _sharedRaceViewModel)))
            {
                raceViewModel.OnDelete += async rvm => await DeleteRace(rvm);
                Races.Add(raceViewModel);
            }
            
            _sharedRaceViewModel.Disciplines.UpdateDataSource(await _logic.GetDisciplines());
            _sharedRaceViewModel.Genders.UpdateDataSource(await _logic.GetGenders());
            _sharedRaceViewModel.Locations.UpdateDataSource(await _logic.GetLocations());
        }
    }
}