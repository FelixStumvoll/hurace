using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using Hurace.RaceControl.ViewModels.Commands;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceItemStartListViewModel : NotifyPropertyChanged
    {
        private IHuraceCore _logic;
        private readonly Race _race;
        private string _availableSearchText;
        private string _selectedSearchText;

        public ObservableCollection<Skier> AvailableSkiers { get; set; } = new ObservableCollection<Skier>();
        public ObservableCollection<StartList> SelectedSkiers { get; set; } = new ObservableCollection<StartList>();
        private readonly List<Skier> _skiers = new List<Skier>();
        private readonly List<StartList> _startList = new List<StartList>();
        private StartList _selectedStartList;

        public string AvailableSearchText
        {
            get => _availableSearchText;
            set => Set(ref _availableSearchText, value);
        }

        public string SelectedSearchText
        {
            get => _selectedSearchText;
            set => Set(ref _selectedSearchText, value);
        }

        public StartList SelectedStartList
        {
            get => _selectedStartList;
            set => Set(ref _selectedStartList, value);
        }

        public ICommand AddSkier { get; set; }
        public ICommand RemoveSkier { get; set; }
        public ICommand AvailableSearchTextChange { get; set; }
        public ICommand SelectedSearchTextChange { get; set; }
        public ICommand StartListUp { get; set; }
        public ICommand StartListDown { get; set; }

        public RaceItemStartListViewModel(IHuraceCore logic, Race race)
        {
            _logic = logic;
            _race = race;

            AddSkier = new AsyncCommand(AddSkierById);
            RemoveSkier = new AsyncCommand(RemoveSkierById);
            AvailableSearchTextChange = new ActionCommand(_ => FillAvailableSkiers(FilterSkier()));

            SelectedSearchTextChange = new ActionCommand(_ => FillSelectedStartList(FilterStartList()));

            StartListUp = new ActionCommand(_ => MoveStartList(i => i - 1),
                                            _ => SelectedStartList != null &&
                                                 _startList.IndexOf(SelectedStartList) != 0);
            StartListDown = new ActionCommand(_ => MoveStartList(i => i + 1),
                                              _ => SelectedStartList != null && 
                                                   _startList.IndexOf(SelectedStartList) != _startList.Count - 1);

            var cntry = new Country {CountryCode = "AT", CountryName = "Österreich"};
            _skiers.Add(new Skier {Id = 1, FirstName = "Felix", LastName = "Stumvoll", Country = cntry});
            _skiers.Add(new Skier {Id = 2, FirstName = "XYZ", LastName = "ABC", Country = cntry});
            _skiers.Add(new Skier {Id = 3, FirstName = "Yeetus", LastName = "Feetus", Country = cntry});

            FillAvailableSkiers(_skiers);
        }

        private IEnumerable<Skier> FilterSkier() =>
            string.IsNullOrEmpty(AvailableSearchText)
                ? _skiers
                : _skiers.Where(
                    s => SkierCompareFunc(s, AvailableSearchText.ToLower()));

        private IEnumerable<StartList> FilterStartList() =>
            string.IsNullOrEmpty(SelectedSearchText)
                ? _startList
                : _startList.Where(
                    s => SkierCompareFunc(s.Skier, SelectedSearchText.ToLower()));

        private static bool SkierCompareFunc(Skier skier, string text) =>
            skier.FirstName.ToLower().Contains(text) ||
            skier.LastName.ToLower().Contains(text) ||
            skier.Country.CountryCode.ToLower().Contains(text) ||
            skier.Country.CountryName.ToLower().Contains(text);

        private void FillAvailableSkiers(IEnumerable<Skier> skiers)
        {
            AvailableSkiers.Clear();
            foreach (var skier in skiers.OrderBy(s => s.LastName)) AvailableSkiers.Add(skier);
        }

        private void FillSelectedStartList(IEnumerable<StartList> startList)
        {
            SelectedSkiers.Clear();
            foreach (var startListEntry in startList.OrderBy(s => s.StartNumber)) SelectedSkiers.Add(startListEntry);
        }

        private void MoveStartList(Func<int, int> operation)
        {
            var index = _startList.IndexOf(SelectedStartList);
            var prev = _startList[operation(index)];
            var tmpSkier = SelectedStartList.Skier;
            SelectedStartList.Skier = prev.Skier;
            prev.Skier = tmpSkier;
            FillSelectedStartList(FilterStartList());
            SelectedStartList = prev;
        }

        private Task AddSkierById(object param)
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
            FillAvailableSkiers(_skiers);
            FillSelectedStartList(_startList);
            return Task.CompletedTask;
        }

        private Task RemoveSkierById(object param)
        {
            var startList = _startList.SingleOrDefault(s => s.SkierId == (int) param);
            if (startList == null) return Task.CompletedTask;
            _skiers.Add(startList.Skier);
            _startList.Remove(startList);
            FillAvailableSkiers(_skiers);
            FillSelectedStartList(_startList);
            return Task.CompletedTask;
        }
    }
}