﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.RaceControl;
using Hurace.Core.Api.RaceCrud;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceViewModel : NotifyPropertyChanged
    {
        public RaceStartListViewModel RaceStartListViewModel { get; set; }
        public RaceBaseDataViewModel RaceBaseDataViewModel { get; set; }
        public RaceControlViewModel RaceControlViewModel { get; set; }
        public Race Race { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand TabSelectionChangedCommand { get; set; }
        private int _selectedTab = -1;
        public bool Edit => RaceBaseDataViewModel.Edit || RaceStartListViewModel.Edit;

        public event Action<RaceViewModel> OnDelete;

        public RaceViewModel(IRaceService logic, Race race, SharedRaceViewModel svm)
        {
            Race = race;
            RaceStartListViewModel = new RaceStartListViewModel(logic, race);
            RaceBaseDataViewModel = new RaceBaseDataViewModel(logic, race, svm);
            RaceControlViewModel =
                new RaceControlViewModel(Race, ServiceProvider.Instance.ResolveService<IRaceControlService>());
            RaceBaseDataViewModel.OnUnsavedCancel += () => OnDelete?.Invoke(this);
            DeleteCommand = new ActionCommand(_ => OnDelete?.Invoke(this), _ => Race.Id != -1);
            TabSelectionChangedCommand = new AsyncCommand(OnTabSelectionChanged);
            RaceBaseDataViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(RaceBaseDataViewModel.Edit)) InvokePropertyChanged(nameof(Edit));
            };

            RaceStartListViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(RaceBaseDataViewModel.Edit)) InvokePropertyChanged(nameof(Edit));
            };
        }

        private async Task OnTabSelectionChanged(object idx)
        {
            var index = (int) idx;
            if (_selectedTab == index) return;
            _selectedTab = index;
            switch (index)
            {
                case 0: await RaceBaseDataViewModel.SetupAsync();
                    break;
                case 1: await RaceStartListViewModel.SetupAsync();
                    break;
                case 2: await RaceControlViewModel.SetupAsync();
                    break;
                case 4: break;
                default: return;
            }
        }

        public async Task SetupAsync()
        {
            await RaceStartListViewModel.SetupAsync();
            await RaceBaseDataViewModel.SetupAsync();
        }
    }
}