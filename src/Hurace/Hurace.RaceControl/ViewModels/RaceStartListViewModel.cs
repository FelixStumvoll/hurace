using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.Race;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceStartListViewModel : NotifyPropertyChanged
    {
        private readonly IRaceService _logic;
        private readonly Race _race;
        private string _skierSearchText;
        private string _startListSearchText;

        public ObservableCollection<Skier> AvailableSkiers { get; set; } = new ObservableCollection<Skier>();
        public ObservableCollection<StartList> StartList { get; set; } = new ObservableCollection<StartList>();
        private readonly List<Skier> _skiers = new List<Skier>();
        private readonly List<StartList> _startList = new List<StartList>();
        private StartList _selectedStartList;

        public string SkierSearchText
        {
            get => _skierSearchText;
            set => Set(ref _skierSearchText, value);
        }

        public string StartListSearchText
        {
            get => _startListSearchText;
            set => Set(ref _startListSearchText, value);
        }

        public StartList SelectedStartList
        {
            get => _selectedStartList;
            set => Set(ref _selectedStartList, value);
        }

        public ICommand AddSkierCommand { get; set; }
        public ICommand RemoveStartListCommand { get; set; }
        public ICommand SkierSearchChangeCommand { get; set; }
        public ICommand StartListSearchChangeCommand { get; set; }
        public ICommand StartListUpCommand { get; set; }
        public ICommand StartListDownCommand { get; set; }

        public RaceStartListViewModel(IRaceService logic, Race race)
        {
            _logic = logic;
            _race = race;

            AddSkierCommand = new AsyncCommand(AddSkier);
            RemoveStartListCommand = new AsyncCommand(RemoveStartList);
            SkierSearchChangeCommand = new ActionCommand(_ => PopulateSkiers(FilterSkier()));

            StartListSearchChangeCommand = new ActionCommand(_ => PopulateStartList(FilterStartList()));

            StartListUpCommand = new ActionCommand(_ => MoveStartList(i => i - 1),
                                            _ => SelectedStartList != null &&
                                                 _startList.IndexOf(SelectedStartList) != 0);
            StartListDownCommand = new ActionCommand(_ => MoveStartList(i => i + 1),
                                              _ => SelectedStartList != null && 
                                                   _startList.IndexOf(SelectedStartList) != _startList.Count - 1);
        }

        public async Task SetupAsync()
        {
            _skiers.Clear();
            _startList.Clear();
            _skiers.AddRange(await _logic.GetAvailableSkiersForRace(_race.Id));
            _startList.AddRange(await _logic.GetStartListForRace(_race.Id));
            PopulateSkiers(_skiers);
            PopulateStartList(_startList);
        }

        private IEnumerable<Skier> FilterSkier() =>
            string.IsNullOrEmpty(SkierSearchText)
                ? _skiers
                : _skiers.Where(
                    s => SkierCompareFunc(s, SkierSearchText.ToLower()));

        private IEnumerable<StartList> FilterStartList() =>
            string.IsNullOrEmpty(StartListSearchText)
                ? _startList
                : _startList.Where(
                    s => SkierCompareFunc(s.Skier, StartListSearchText.ToLower()));

        private static bool SkierCompareFunc(Skier skier, string text) =>
            skier.FirstName.ToLower().Contains(text) ||
            skier.LastName.ToLower().Contains(text) ||
            skier.Country.CountryCode.ToLower().Contains(text) ||
            skier.Country.CountryName.ToLower().Contains(text);

        private void PopulateSkiers(IEnumerable<Skier> skiers)
        {
            AvailableSkiers.Clear();
            foreach (var skier in skiers.OrderBy(s => s.LastName)) AvailableSkiers.Add(skier);
        }

        private void PopulateStartList(IEnumerable<StartList> startList)
        {
            StartList.Clear();
            foreach (var startListEntry in startList.OrderBy(s => s.StartNumber)) StartList.Add(startListEntry);
        }

        private void MoveStartList(Func<int, int> operation)
        {
            var index = _startList.IndexOf(SelectedStartList);
            var prev = _startList[operation(index)];
            var tmpSkier = SelectedStartList.Skier;
            SelectedStartList.Skier = prev.Skier;
            prev.Skier = tmpSkier;
            PopulateStartList(FilterStartList());
            SelectedStartList = prev;
        }

        private Task AddSkier(object param)
        {
            var skier = _skiers.SingleOrDefault(s => s.Id == (int) param);
            if (skier == null) return Task.CompletedTask;
            _startList.Add(new StartList
            {
                Race = _race,
                RaceId = _race.Id,
                Skier = skier,
                SkierId = skier.Id,
                StartStateId = (int) Constants.StartState.DrawReady,
                StartNumber = _startList.Count + 1
            });

            _skiers.Remove(skier);
            PopulateSkiers(_skiers);
            PopulateStartList(_startList);
            return Task.CompletedTask;
        }

        private Task RemoveStartList(object param)
        {
            var startList = _startList.SingleOrDefault(s => s.SkierId == (int) param);
            if (startList == null) return Task.CompletedTask;
            _skiers.Add(startList.Skier);
            _startList.Remove(startList);
            PopulateSkiers(_skiers);
            PopulateStartList(_startList);
            return Task.CompletedTask;
        }
    }
}