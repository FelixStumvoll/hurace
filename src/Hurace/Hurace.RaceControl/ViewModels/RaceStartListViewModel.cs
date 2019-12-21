using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hurace.Core.Logic.RaceStartListService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.Util;
using StartState = Hurace.Dal.Domain.Enums.StartState;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceStartListViewModel : NotifyPropertyChanged
    {
        private readonly IRaceStartListService _startListService;
        private StartList _selectedStartList;

        public SharedRaceStateViewModel RaceState { get; set; }
        public FilterableObservableCollection<Skier> AvailableSkiers { get; set; }
        public FilterableObservableCollection<StartList> StartList { get; set; }

        public ICommand AddSkierCommand { get; set; }
        public ICommand RemoveStartListCommand { get; set; }
        public ICommand StartListUpCommand { get; set; }
        public ICommand StartListDownCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }

        public StartList SelectedStartList
        {
            get => _selectedStartList;
            set => Set(ref _selectedStartList, value);
        }

        public RaceStartListViewModel(SharedRaceStateViewModel raceState, IRaceStartListService startListService)
        {
            RaceState = raceState;
            _startListService = startListService;
            SetupCommands();
        }

        private bool CanMoveUp() =>
            SelectedStartList != null &&
            StartList.DataSource.IndexOf(SelectedStartList) != 0 &&
            RaceState.Edit;

        private bool CanMoveDown() =>
            SelectedStartList != null &&
            StartList.DataSource.IndexOf(SelectedStartList) !=
            StartList.DataSource.Count - 1 &&
            RaceState.Edit;

        private void SetupCommands()
        {
            EditCommand = new RelayCommand(EditStartList,
                                            () => RaceState.Race.RaceStateId ==
                                                 (int) Dal.Domain.Enums.RaceState.Upcoming);
            SaveCommand = new AsyncCommand(SaveStartList);
            CancelEditCommand = new AsyncCommand(CancelEditStartList);

            AddSkierCommand = new RelayCommand<int>(AddSkier, _ => RaceState.Edit);
            RemoveStartListCommand = new RelayCommand<int>(RemoveStartList, _ => RaceState.Edit);

            StartListUpCommand = new RelayCommand(() => MoveStartList(i => i - 1), CanMoveUp);
            StartListDownCommand = new RelayCommand(() => MoveStartList(i => i + 1), CanMoveDown);
            AvailableSkiers =
                new FilterableObservableCollection<Skier>(SkierFilterFunc,
                                                          s =>
                                                              s.OrderBy(sk => sk.LastName));
            StartList = new FilterableObservableCollection<StartList>((sl, term) =>
                                                                          SkierFilterFunc(sl.Skier, term),
                                                                      l =>
                                                                          l.OrderBy(sl => sl.StartNumber));
        }

        public async Task SetupAsync()
        {
            try
            {
                var skiers = await _startListService.GetAvailableSkiersForRace(RaceState.Race.Id);
                AvailableSkiers.UpdateDataSource(skiers);
                AvailableSkiers.Apply();
                var startList = await _startListService.GetStartListForRace(RaceState.Race.Id);
                StartList.UpdateDataSource(startList);
                StartList.Apply();
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }

        private static bool SkierFilterFunc(Skier skier, string text) =>
            skier.FirstName.ToLower().Contains(text) ||
            skier.LastName.ToLower().Contains(text) ||
            skier.Country.CountryCode.ToLower().Contains(text) ||
            skier.Country.CountryName.ToLower().Contains(text);

        private void MoveStartList(Func<int, int> operation)
        {
            var index = StartList.DataSource.IndexOf(SelectedStartList);
            var prev = StartList.DataSource[operation(index)];
            var tmpSkier = SelectedStartList.Skier;
            SelectedStartList.Skier = prev.Skier;
            prev.Skier = tmpSkier;
            StartList.Apply();
            SelectedStartList = prev;
        }

        private void AddSkier(int skierId)
        {
            var skier = AvailableSkiers.DataSource.SingleOrDefault(s => s.Id == skierId);
            if (skier == null) return;

            StartList.DataSource.Add(new StartList
            {
                Race = RaceState.Race,
                RaceId = RaceState.Race.Id,
                Skier = skier,
                SkierId = skier.Id,
                StartStateId = (int) StartState.Upcoming,
                StartNumber = StartList.DataSource.Count + 1
            });

            AvailableSkiers.DataSource.Remove(skier);
            AvailableSkiers.Apply();
            StartList.Apply();
        }

        private void RemoveStartList(int skierId)
        {
            var startList = StartList.DataSource.SingleOrDefault(s => s.SkierId == skierId);
            if (startList == null) return;

            AvailableSkiers.DataSource.Add(startList.Skier);
            StartList.DataSource.Remove(startList);
            foreach (var sl in StartList.DataSource
                                        .Where(s => s.StartNumber > startList.StartNumber)) sl.StartNumber--;
            AvailableSkiers.Apply();
            StartList.Apply();
        }

        private async Task SaveStartList()
        {
            try
            {
                await _startListService.UpdateStartList(RaceState.Race.Id, StartList.DataSource);
                RaceState.Edit = false;
            }
            catch (Exception)
            {
                ErrorNotifier.OnSaveError();
            }
        }

        private Task CancelEditStartList()
        {
            RaceState.Edit = false;
            return SetupAsync();
        }

        private void EditStartList() => RaceState.Edit = true;
    }
}