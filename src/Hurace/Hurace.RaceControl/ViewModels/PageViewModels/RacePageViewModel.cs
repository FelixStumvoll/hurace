using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.PageViewModels
{
    public class RacePageViewModel : NotifyPropertyChanged, IPage
    {
        private RaceViewModel _selectedRace;
        private readonly IRaceBaseDataService _baseDataService;
        private readonly ISeasonService _seasonService;
        private readonly SharedRaceViewModel _sharedRaceViewModel;
        private Season _selectedSeason;
        private ICommand _seasonChangedCommand;
        private readonly Func<Race, bool, RaceViewModel> _raceVmFactory;

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

        public RacePageViewModel(Func<Race, bool, RaceViewModel> raceVmFactory,
            SharedRaceViewModel sharedRaceViewModel,
            IRaceBaseDataService baseDataService,
            ISeasonService seasonService)
        {
            _raceVmFactory = raceVmFactory;
            _baseDataService = baseDataService;
            _seasonService = seasonService;
            _sharedRaceViewModel = sharedRaceViewModel;
            SetupCommands();
        }

        public async Task SetupAsync()
        {
            try
            {
                var seasons = await _seasonService.GetAllSeasons();
                var seasonList = seasons.ToList();
                if (!seasonList.Any()) return;
                Seasons.Repopulate(seasonList.OrderByDescending(s => s.StartDate));
                SelectedSeason = Seasons[0];
                SeasonChangedCommand ??=
                    new AsyncCommand(LoadRaces);
                await LoadRaces();
                _sharedRaceViewModel.Genders.UpdateDataSource(await _baseDataService.GetGenders());
                _sharedRaceViewModel.Locations.UpdateDataSource(await _baseDataService.GetLocations());
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
                var races = await _seasonService.GetRacesForSeason(SelectedSeason.Id);
                Races.Clear();
                foreach (var raceViewModel in races
                    .Select(r => _raceVmFactory(r, false)))
                {
                    raceViewModel.OnDelete += async rvm => await DeleteRace(rvm);
                    Races.Add(raceViewModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                ErrorNotifier.OnSaveError();
            }
        }

        private void SetupCommands()
        {
            AddRaceCommand = new RelayCommand(AddRace);
            SelectedRaceChangedCommand = new AsyncCommand(async () =>
            {
                if (SelectedRace == null) return;
                await SelectedRace.SetupAsync();
            });
        }

        private void AddRace()
        {
            var rvm = _raceVmFactory(new Race
            {
                Id = -1,
                RaceStateId = (int) Dal.Domain.Enums.RaceState.Upcoming,
                RaceDate = DateTime.Now,
                SeasonId = SelectedSeason.Id,
                Season = SelectedSeason
            }, true);
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
            if (rvm.RaceState.Race.Id != -1 && !await _baseDataService.RemoveRace(rvm.RaceState.Race))
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