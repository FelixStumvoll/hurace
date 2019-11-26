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
        private Race _race;
        private string _availableSearchText;
        private string _selectedSearchText;
        private Skier _selectedSkier;
        private StartList _selectedStartList;

        public ObservableCollection<Skier> AvailableSkiers { get; set; } = new ObservableCollection<Skier>();
        public ObservableCollection<StartList> SelectedSkiers { get; set; } = new ObservableCollection<StartList>();

        public Skier SelectedSkier
        {
            get => _selectedSkier;
            set => Set(ref _selectedSkier, value);
        }

        public StartList SelectedStartList
        {
            get => _selectedStartList;
            set => Set(ref _selectedStartList, value);
        }

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

        public ICommand AddSkier { get; set; }
        public ICommand RemoveSkier { get; set; }

        public RaceItemStartListViewModel(IHuraceCore logic, Race race)
        {
            _logic = logic;
            _race = race;

            AddSkier = new AsyncCommand(AddSkiers, _ => SelectedSkier != null);
            RemoveSkier = new AsyncCommand(RemoveSkiers, _ => SelectedStartList != null);

            AvailableSkiers.Add(new Skier {FirstName = "Felix", LastName = "Stumvoll"});
            AvailableSkiers.Add(new Skier {FirstName = "XYZ", LastName = "ABC"});
            AvailableSkiers.Add(new Skier {FirstName = "Yeetus", LastName = "Feetus"});
        }

        private IEnumerable<T> CastParam<T>(object param)
        {
            var castedList = new List<T>();
            castedList.AddRange(((IList) param).Cast<T>());
            return castedList;
        }

        private Task AddSkiers(object param)
        {
            foreach (var skier in CastParam<Skier>(param))
            {
                SelectedSkiers.Add(new StartList
                {
                    Race = _race,
                    RaceId = _race.Id,
                    Skier = skier,
                    SkierId = skier.Id,
                    StartStateId = (int) Constants.StartState.DrawReady
                });

                AvailableSkiers.Remove(skier);
            }

            SelectedSkier = null;
            return Task.CompletedTask;
        }

        private Task RemoveSkiers(object param)
        {
            foreach (var startList in CastParam<StartList>(param))
            {
                AvailableSkiers.Add(startList.Skier);
                SelectedSkiers.Remove(startList);
            }

            SelectedStartList = null;
            return Task.CompletedTask;
        }
    }
}