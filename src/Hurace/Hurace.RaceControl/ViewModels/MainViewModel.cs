using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.Race;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private RaceViewModel _selectedRace;

        private readonly IRaceService _logic;
        private readonly SharedRaceViewModel _sharedRaceViewModel;

        public ObservableCollection<RaceViewModel> AllRaces { get; set; } =
            new ObservableCollection<RaceViewModel>();

        public ObservableCollection<RaceViewModel> ActiveRaces { get; set; } =
            new ObservableCollection<RaceViewModel>();

        public RaceViewModel SelectedRace
        {
            get => _selectedRace;
            set => Set(ref _selectedRace, value);
        }

        public ICommand AddRaceCommand { get; set; }
        public ICommand SelectedRaceChangedCommand { get; set; }

        public MainViewModel(IRaceService logic)
        {
            _logic = logic;
            _sharedRaceViewModel = new SharedRaceViewModel();
            AddRaceCommand = new ActionCommand(_ =>
            {
                var rvm = new RaceViewModel(_logic, new Race(), _sharedRaceViewModel, DeleteRace, true)
                    {Edit = true};
                AllRaces.Add(rvm);
                SelectedRace = rvm;
            });

            SelectedRaceChangedCommand = new ActionCommand(async _ => { await SelectedRace.SetupAsync(); });
        }

        private bool DeleteRace(RaceViewModel rvm)
        {
            if (MessageBox.Show("Delete Race ?", "Delete ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return false;
            AllRaces.Remove(SelectedRace);
            ActiveRaces.Remove(SelectedRace);
            SelectedRace = null;
            return true;
        }

        public async Task InitializeAsync()
        {
            AllRaces.AddRange(
                (await _logic.GetAllRaces()).Select(
                    r => new RaceViewModel(_logic, r, _sharedRaceViewModel, DeleteRace)));
            ActiveRaces.AddRange(
                (await _logic.GetActiveRaces()).Select(
                    r => new RaceViewModel(_logic, r, _sharedRaceViewModel, DeleteRace)));
            _sharedRaceViewModel.Disciplines.AddRange(await _logic.GetDisciplines());
            _sharedRaceViewModel.Genders.AddRange(await _logic.GetGenders());
            _sharedRaceViewModel.Locations.AddRange(await _logic.GetLocations());
        }
    }
}