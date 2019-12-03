using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.RaceCrud;
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

        public FilterableObservableCollection<Skier> AvailableSkiers { get; set; }

        public FilterableObservableCollection<StartList> StartList { get; set; }

        private StartList _selectedStartList;

        public StartList SelectedStartList
        {
            get => _selectedStartList;
            set => Set(ref _selectedStartList, value);
        }

        public ICommand AddSkierCommand { get; set; }
        public ICommand RemoveStartListCommand { get; set; }
        public ICommand StartListUpCommand { get; set; }
        public ICommand StartListDownCommand { get; set; }

        public RaceStartListViewModel(IRaceService logic, Race race)
        {
            _logic = logic;
            _race = race;

            AddSkierCommand = new AsyncCommand(AddSkier);
            RemoveStartListCommand = new AsyncCommand(RemoveStartList);

            StartListUpCommand = new ActionCommand(_ => MoveStartList(i => i - 1),
                                                   _ => SelectedStartList != null &&
                                                        StartList.DataSource.IndexOf(SelectedStartList) != 0);
            StartListDownCommand = new ActionCommand(_ => MoveStartList(i => i + 1),
                                                     _ => SelectedStartList != null &&
                                                          StartList.DataSource.IndexOf(SelectedStartList) !=
                                                          StartList.DataSource.Count - 1);
            AvailableSkiers =
                new FilterableObservableCollection<Skier>(SkierFilterFunc, s => s.OrderBy(sk => sk.LastName));
            StartList = new FilterableObservableCollection<StartList>((sl, st) => SkierFilterFunc(sl.Skier, st),
                                                                      l => l.OrderBy(sl => sl.StartNumber));
        }

        public async Task SetupAsync()
        {
            AvailableSkiers.UpdateDataSource(await _logic.GetAvailableSkiersForRace(_race.Id));
            AvailableSkiers.Apply();
            StartList.UpdateDataSource(await _logic.GetStartListForRace(_race.Id));
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

        private Task AddSkier(object param)
        {
            var skier = AvailableSkiers.DataSource.SingleOrDefault(s => s.Id == (int) param);
            if (skier == null) return Task.CompletedTask;
            StartList.DataSource.Add(new StartList
            {
                Race = _race,
                RaceId = _race.Id,
                Skier = skier,
                SkierId = skier.Id,
                StartStateId = (int) Constants.StartState.DrawReady,
                StartNumber = StartList.DataSource.Count + 1
            });

            AvailableSkiers.DataSource.Remove(skier);
            AvailableSkiers.Apply();
            StartList.Apply();
            return Task.CompletedTask;
        }

        private Task RemoveStartList(object param)
        {
            var startList = StartList.DataSource.SingleOrDefault(s => s.SkierId == (int) param);
            if (startList == null) return Task.CompletedTask;
            AvailableSkiers.DataSource.Add(startList.Skier);
            StartList.DataSource.Remove(startList);
            foreach (var sl in StartList.DataSource.Where(s => s.StartNumber > startList.StartNumber)) sl.StartNumber--;
            AvailableSkiers.Apply();
            StartList.Apply();
            return Task.CompletedTask;
        }
    }
}