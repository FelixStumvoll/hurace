﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hurace.Core.Api.RaceCrud;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
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
            var seasons = (await _logic.GetAllSeasons()).OrderByDescending(s => s.StartDate);
            Seasons.Clear();
            Seasons.AddRange(seasons);
            if (!Seasons.Any()) return;
            SelectedSeason = Seasons[0];
            await LoadRaces();

            _sharedRaceViewModel.Disciplines.UpdateDataSource(await _logic.GetDisciplines());
            _sharedRaceViewModel.Genders.UpdateDataSource(await _logic.GetGenders());
            _sharedRaceViewModel.Locations.UpdateDataSource(await _logic.GetLocations());

            if (SeasonChangedCommand == null) SeasonChangedCommand = new AsyncCommand(async _ => await LoadRaces());
        }

        private async Task LoadRaces()
        {
            var races = (await _logic.GetRacesForSeason(SelectedSeason.Id)).Select(
                r => new RaceViewModel(_logic, r, _sharedRaceViewModel));
            Races.Clear();
            foreach (var raceViewModel in races)
            {
                raceViewModel.OnDelete += async rvm => await DeleteRace(rvm);
                Races.Add(raceViewModel);
            }
        }

        private void SetupCommands()
        {
            AddRaceCommand = new ActionCommand(_ =>
            {
                var rvm = new RaceViewModel(
                    _logic,
                    new Race
                    {
                        Id = -1,
                        RaceStateId = (int) Constants.RaceState.Upcoming,
                        RaceDate = DateTime.Now,
                        SeasonId = SelectedSeason.Id,
                        Season = SelectedSeason
                    },
                    _sharedRaceViewModel);
                rvm.OnDelete += async deleteRvm => await DeleteRace(deleteRvm);
                Races.Add(rvm);
                SelectedRace = rvm;
            });

            SelectedRaceChangedCommand = new AsyncCommand(async _ =>
            {
                if (SelectedRace == null) return;
                await SelectedRace.SetupAsync();
            });
        }

        private async Task DeleteRace(RaceViewModel rvm)
        {
            if (MessageBox.Show("Rennen löschen ?",
                                "Löschen ?",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Information) != MessageBoxResult.Yes) return;
            if (rvm.Race.Id != -1 && !await _logic.RemoveRace(rvm.Race))
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