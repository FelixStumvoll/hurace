using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceStartListViewModel : NotifyPropertyChanged
    {
        private readonly IRaceService _logic;
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

        public RaceStartListViewModel(IRaceService logic, SharedRaceStateViewModel raceState)
        {
            _logic = logic;
            RaceState = raceState;
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
            EditCommand = new ActionCommand(_ => EditStartList());
            SaveCommand = new AsyncCommand(_ => SaveStartList());
            CancelEditCommand = new AsyncCommand(_ => CancelEditStartList());

            AddSkierCommand = new ActionCommand(AddSkier, _ => RaceState.Edit);
            RemoveStartListCommand = new ActionCommand(RemoveStartList, _ => RaceState.Edit);

            StartListUpCommand = new ActionCommand(_ => MoveStartList(i => i - 1),
                                                   _ => CanMoveUp());
            StartListDownCommand = new ActionCommand(_ => MoveStartList(i => i + 1),
                                                     _ => CanMoveDown());
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
            var skiers = await _logic.GetAvailableSkiersForRace(RaceState.Race.Id);
            var startLists = await _logic.GetStartListForRace(RaceState.Race.Id);
            if (skiers == null || startLists == null)
            {
                ErrorNotifier.OnLoadError();
                return;
            }

            AvailableSkiers.UpdateDataSource(skiers);
            StartList.UpdateDataSource(startLists);
            AvailableSkiers.Apply();
            StartList.Apply();
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

        private void AddSkier(object skierId)
        {
            var skier = AvailableSkiers.DataSource.SingleOrDefault(s => s.Id == (int) skierId);
            if (skier == null) return;

            StartList.DataSource.Add(new StartList
            {
                Race = RaceState.Race,
                RaceId = RaceState.Race.Id,
                Skier = skier,
                SkierId = skier.Id,
                StartStateId = (int) Constants.StartState.Upcoming,
                StartNumber = StartList.DataSource.Count + 1
            });

            AvailableSkiers.DataSource.Remove(skier);
            AvailableSkiers.Apply();
            StartList.Apply();
        }

        private void RemoveStartList(object skierId)
        {
            var startList = StartList.DataSource.SingleOrDefault(s => s.SkierId == (int) skierId);
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
            if (await _logic.UpdateStartList(RaceState.Race, StartList.DataSource))
            {
                RaceState.Edit = false;
                return;
            }

            ErrorNotifier.OnSaveError();
        }

        private Task CancelEditStartList()
        {
            RaceState.Edit = false;
            return SetupAsync();
        }

        private void EditStartList() => RaceState.Edit = true;
    }
}