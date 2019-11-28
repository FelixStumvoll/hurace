using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Dto;
using Hurace.RaceControl.Util;
using Hurace.RaceControl.ViewModels.Commands;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private RaceItemViewModel _currentRace;

        private readonly IHuraceCore _logic;
        private readonly SharedRaceItemViewModel _sharedRaceItemViewModel;

        public ObservableCollection<RaceItemViewModel> AllRaces { get; set; } =
            new ObservableCollection<RaceItemViewModel>();

        public ObservableCollection<RaceItemViewModel> ActiveRaces { get; set; } =
            new ObservableCollection<RaceItemViewModel>();

        public RaceItemViewModel CurrentRace
        {
            get => _currentRace;
            set => Set(ref _currentRace, value);
        }

        public ICommand AddRace { get; set; }
        public ICommand SelectedRaceChanged { get; set; }

        public MainViewModel(IHuraceCore logic)
        {
            _logic = logic;
            _sharedRaceItemViewModel = new SharedRaceItemViewModel();
            AddRace = new ActionCommand(_ =>
            {
                var rvm = new RaceItemViewModel(_logic, new Race(), _sharedRaceItemViewModel, DeleteRace) {Edit = true};
                AllRaces.Add(rvm);
                CurrentRace = rvm;
            });

            SelectedRaceChanged = new ActionCommand(_ => { });
        }

        private bool DeleteRace(RaceItemViewModel rvm)
        {
            if (MessageBox.Show("Delete Race ?", "Delete ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return false;
            AllRaces.Remove(CurrentRace);
            ActiveRaces.Remove(CurrentRace);
            CurrentRace = null;
            return true;
        }

        public async Task InitializeAsync()
        {
            AllRaces.AddRange(
                (await _logic.GetAllRaces()).Select(
                    r => new RaceItemViewModel(_logic, r, _sharedRaceItemViewModel, DeleteRace)));
            ActiveRaces.AddRange(
                (await _logic.GetActiveRaces()).Select(
                    r => new RaceItemViewModel(_logic, r, _sharedRaceItemViewModel, DeleteRace)));
            _sharedRaceItemViewModel.Disciplines.AddRange(await _logic.GetDisciplines());
            _sharedRaceItemViewModel.Genders.AddRange(await _logic.GetGenders());
            _sharedRaceItemViewModel.Locations.AddRange(await _logic.GetLocations());
        }
    }
}