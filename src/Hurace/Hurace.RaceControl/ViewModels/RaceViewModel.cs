using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Services.RaceBaseDataService;
using Hurace.Core.Logic.Services.RaceStartListService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.RaceControlViewModels;
using Hurace.RaceControl.ViewModels.SharedViewModels;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceViewModel : NotifyPropertyChanged
    {
        private int _selectedTab;
        private int _tempTabIndex;
        public event Action<RaceViewModel> OnDelete;
        public RaceStartListViewModel RaceStartListViewModel { get; set; }
        public RaceBaseDataViewModel RaceBaseDataViewModel { get; set; }
        public RaceControlBaseViewModel RaceControlBaseViewModel { get; set; }
        public RaceDisplayViewModel RaceDisplayViewModel { get; set; }
        public SharedRaceStateViewModel RaceState { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand TabSelectionChangedCommand { get; set; }

        public int SelectedTab
        {
            get => _selectedTab;
            set => Set(ref _selectedTab, value);
        }

        public RaceViewModel(Race race, bool edit,
            Func<SharedRaceStateViewModel, RaceStartListViewModel> raceStartListVmFactory,
            Func<SharedRaceStateViewModel, RaceBaseDataViewModel> raceBaseDataVmFactory,
            Func<SharedRaceStateViewModel, RaceControlBaseViewModel> raceControlBaseVmFactory,
            Func<SharedRaceStateViewModel, RaceDisplayViewModel> raceDisplayVmFactory)
        {
            RaceState = new SharedRaceStateViewModel{Edit =  edit, Race = race};
            RaceStartListViewModel = raceStartListVmFactory(RaceState);
            RaceBaseDataViewModel = raceBaseDataVmFactory(RaceState);
            RaceControlBaseViewModel = raceControlBaseVmFactory(RaceState);
            RaceDisplayViewModel = raceDisplayVmFactory(RaceState);
            SetupCommands();
        }

        private void SetupCommands()
        {
            RaceBaseDataViewModel.OnUnsavedCancel += () => OnDelete?.Invoke(this);
            DeleteCommand = new RelayCommand(() => OnDelete?.Invoke(this),
                                             () => RaceState.Race.Id != -1 &&
                                                   RaceState.Race.RaceStateId ==
                                                   (int) Dal.Domain.Enums.RaceState.Upcoming);
            TabSelectionChangedCommand = new AsyncCommand(OnTabSelectionChanged);
        }

        public async Task SetupAsync() => await SetupTab();

        private async Task OnTabSelectionChanged()
        {
            if (_tempTabIndex == SelectedTab) return;
            await SetupTab();
            _tempTabIndex = SelectedTab;
        }

        private async Task SetupTab()
        {
            switch (SelectedTab)
            {
                case 0:
                    await RaceBaseDataViewModel.SetupAsync();
                    break;
                case 1:
                    await RaceStartListViewModel.SetupAsync();
                    break;
                case 2:
                    await RaceControlBaseViewModel.SetupAsync();
                    break;
                case 4: break;
                default: return;
            }
        }
    }
}