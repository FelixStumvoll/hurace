using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api.RaceService;
using Hurace.Core.Api.Util;
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
            (await _logic.GetAllSeasons())
                .Map(seasons =>
                {
                    var seasonList = seasons.ToList();
                    if (!seasonList.Any()) return Result<object, Exception>.Err(null);
                    Seasons.Clear();
                    Seasons.AddRange(seasonList.OrderByDescending(s => s.StartDate));
                    SelectedSeason = Seasons[0];
                    SeasonChangedCommand ??=
                        new AsyncCommand(async _ => (await LoadRaces()).OrElse(_ => ErrorNotifier.OnLoadError()));
                    return Result<object, Exception>.Ok(null);
                })
                .And(await LoadRaces())
                .And(await _logic.GetDisciplines(), disciplines =>
                         _sharedRaceViewModel.Disciplines.UpdateDataSource(disciplines))
                .And(await _logic.GetGenders(), genders =>
                         _sharedRaceViewModel.Genders.UpdateDataSource(genders))
                .And(await _logic.GetLocations(), locations =>
                         _sharedRaceViewModel.Locations.UpdateDataSource(locations))
                .OrElse(_ => ErrorNotifier.OnLoadError());
        }

        private async Task<Result<IEnumerable<Race>, Exception>> LoadRaces() =>
            (await _logic.GetRacesForSeason(SelectedSeason.Id))
            .AndThen(races =>
            {
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
            });

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
                        RaceStateId = (int) Constants.RaceState.Upcoming,
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
            if (rvm.RaceState.Race.Id != -1 && (await _logic.RemoveRace(rvm.RaceState.Race)).Failure)
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