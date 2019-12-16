using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.PageViewModels
{
    public class RacePageViewModel : NotifyPropertyChanged, IPageViewModel
    {
        private RaceViewModel _selectedRace;
        private readonly IRaceService _logic;
        private readonly SharedRaceViewModel _sharedRaceViewModel;
        private Season _selectedSeason;
        private ICommand _seasonChangedCommand;

        public ObservableCollection<RaceViewModel> Races { get; set; } = new ObservableCollection<RaceViewModel>();
        public ObservableCollection<Season> Seasons { get; set; } = new ObservableCollection<Season>();
        public ICommand AddRaceCommand { get; set; }
        public ICommand SelectedRaceChangedCommand { get; set; }

        public ICommand SeasonChangedCommand
        {
            get => _seasonChangedCommand;
            set => Set(ref _seasonChangedCommand, value);
        }

        public RaceViewModel SelectedRace
        {
            get => _selectedRace;
            set => Set(ref _selectedRace, value);
        }

        public Season SelectedSeason
        {
            get => _selectedSeason;
            set => Set(ref _selectedSeason, value);
        }

        public RacePageViewModel(IRaceService logic)
        {
            _logic = logic;
            _sharedRaceViewModel = new SharedRaceViewModel();
            SetupCommands();
        }

        public async Task SetupAsync()
        {
            try
            {
                var seasons = await _logic.GetAllSeasons();
                var seasonList = seasons.ToList();
                if (!seasonList.Any()) return;
                Seasons.Repopulate(seasonList.OrderByDescending(s => s.StartDate));
                SelectedSeason = Seasons[0];
                SeasonChangedCommand ??=
                    new AsyncCommand(async _ => await LoadRaces());
                await LoadRaces();
                _sharedRaceViewModel.Disciplines.UpdateDataSource(await _logic.GetDisciplines());
                _sharedRaceViewModel.Genders.UpdateDataSource(await _logic.GetGenders());
                _sharedRaceViewModel.Locations.UpdateDataSource(await _logic.GetLocations());
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }

        private async Task LoadRaces()
        {
            try
            {
                var races = await _logic.GetRacesForSeason(SelectedSeason.Id);
                Races.Clear();
                foreach (var raceViewModel in races
                    .Select(r =>
                                new RaceViewModel(
                                    _logic, new SharedRaceStateViewModel {Race = r},
                                    _sharedRaceViewModel)))
                {
                    raceViewModel.OnDelete += async rvm => await DeleteRace(rvm);
                    Races.Add(raceViewModel);
                }
            }
            catch (Exception)
            {
                ErrorNotifier.OnSaveError();
            }
        }

        private void SetupCommands()
        {
            AddRaceCommand = new ActionCommand(_ => AddRace());
            SelectedRaceChangedCommand = new AsyncCommand(async _ =>
            {
                if (SelectedRace == null) return;
                await SelectedRace.SetupAsync();
            });
        }

        private void AddRace()
        {
            var rvm = new RaceViewModel(
                _logic,
                new SharedRaceStateViewModel
                {
                    Race = new Race
                    {
                        Id = -1,
                        RaceStateId = (int) Dal.Domain.Enums.RaceState.Upcoming,
                        RaceDate = DateTime.Now,
                        SeasonId = SelectedSeason.Id,
                        Season = SelectedSeason
                    },
                    Edit = true
                },
                _sharedRaceViewModel);
            rvm.OnDelete += async deleteRvm => await DeleteRace(deleteRvm);
            Races.Add(rvm);
            SelectedRace = rvm;
        }

        private async Task DeleteRace(RaceViewModel rvm)
        {
            if (MessageBox.Show("Rennen löschen ?",
                                "Löschen ?",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Information) != MessageBoxResult.Yes) return;
            if (rvm.RaceState.Race.Id != -1 && !await _logic.RemoveRace(rvm.RaceState.Race))
            {
                MessageBox.Show("Rennen konnte nicht gelöscht werden!",
                                "Fehler",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            DeleteRaceViewModel(rvm);
        }

        private void DeleteRaceViewModel(RaceViewModel rvm)
        {
            Races.Remove(rvm);
            SelectedRace = null;
        }
    }
}