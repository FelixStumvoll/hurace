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

        private void SetupCommands()
        {
            EditCommand = new ActionCommand(_ => EditStartList());
            SaveCommand = new AsyncCommand(_ => SaveStartList());
            CancelEditCommand = new AsyncCommand(_ => CancelEditStartList());

            AddSkierCommand = new ActionCommand(AddSkier, _ => RaceState.Edit);
            RemoveStartListCommand = new ActionCommand(RemoveStartList, _ => RaceState.Edit);

            StartListUpCommand = new ActionCommand(_ => MoveStartList(i => i - 1),
                                                   _ => SelectedStartList != null &&
                                                        StartList.DataSource.IndexOf(SelectedStartList) != 0 &&
                                                        RaceState.Edit);
            StartListDownCommand = new ActionCommand(_ => MoveStartList(i => i + 1),
                                                     _ => SelectedStartList != null &&
                                                          StartList.DataSource.IndexOf(SelectedStartList) !=
                                                          StartList.DataSource.Count - 1 &&
                                                          RaceState.Edit);
            AvailableSkiers =
                new FilterableObservableCollection<Skier>(SkierFilterFunc, s => s.OrderBy(sk => sk.LastName));
            StartList = new FilterableObservableCollection<StartList>((sl, st) => SkierFilterFunc(sl.Skier, st),
                                                                      l => l.OrderBy(sl => sl.StartNumber));
        }

        public async Task SetupAsync()
        {
            AvailableSkiers.UpdateDataSource(await _logic.GetAvailableSkiersForRace(RaceState.Race.Id));
            StartList.UpdateDataSource(await _logic.GetStartListForRace(RaceState.Race.Id));
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

        private void AddSkier(object param)
        {
            var skier = AvailableSkiers.DataSource.SingleOrDefault(s => s.Id == (int) param);
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

        private void RemoveStartList(object param)
        {
            var startList = StartList.DataSource.SingleOrDefault(s => s.SkierId == (int) param);
            if (startList == null) return;
            AvailableSkiers.DataSource.Add(startList.Skier);
            StartList.DataSource.Remove(startList);
            foreach (var sl in StartList.DataSource.Where(s => s.StartNumber > startList.StartNumber)) sl.StartNumber--;
            AvailableSkiers.Apply();
            StartList.Apply();
        }

        private Task SaveStartList()
        {
            RaceState.Edit = false;
            return _logic.UpdateStartList(RaceState.Race, StartList.DataSource);
        }

        private Task CancelEditStartList()
        {
            RaceState.Edit = false;
            return SetupAsync();
        }

        private void EditStartList() => RaceState.Edit = true;
    }
}