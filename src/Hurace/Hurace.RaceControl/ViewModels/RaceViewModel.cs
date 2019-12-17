using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api.RaceService;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.RaceControlViewModels;
using Hurace.RaceControl.ViewModels.Util;

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

        public RaceViewModel(IRaceService logic, SharedRaceStateViewModel raceState, SharedRaceViewModel svm)
        {
            RaceState = raceState;
            RaceStartListViewModel = new RaceStartListViewModel(logic, RaceState);
            RaceBaseDataViewModel = new RaceBaseDataViewModel(logic, svm,RaceState);
            RaceControlBaseViewModel = new RaceControlBaseViewModel(RaceState, logic);
            RaceDisplayViewModel = new RaceDisplayViewModel(RaceState);

            SetupCommands();
        }

        private void SetupCommands()
        {
            RaceBaseDataViewModel.OnUnsavedCancel += () => OnDelete?.Invoke(this);
            DeleteCommand = new ActionCommand(_ => OnDelete?.Invoke(this),
                                              _ => RaceState.Race.Id !=
                                                   -1); // && Race.RaceStateId == (int) Constants.RaceState.Upcoming
            TabSelectionChangedCommand = new AsyncCommand(_ => OnTabSelectionChanged());
        }

        public async Task SetupAsync() => await SetupTab();

        private async Task OnTabSelectionChanged(bool ignoreSameTab = true)
        {
            if (_tempTabIndex == SelectedTab && ignoreSameTab) return;
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