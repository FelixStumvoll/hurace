using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hurace.Core.Logic;
using Hurace.Core.Logic.RaceBaseDataService;
using Hurace.Core.Logic.RaceService;
using Hurace.Core.Logic.RaceStartListService;
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

        public RaceViewModel(SharedRaceStateViewModel raceState, SharedRaceViewModel svm)
        {
            RaceState = raceState;
            var provider = ServiceProvider.Instance;
            RaceStartListViewModel = new RaceStartListViewModel(RaceState, provider.Resolve<IRaceStartListService>());
            RaceBaseDataViewModel = new RaceBaseDataViewModel(svm, RaceState, provider.Resolve<IRaceBaseDataService>());
            RaceControlBaseViewModel = new RaceControlBaseViewModel(RaceState, provider.Resolve<IRaceBaseDataService>(),
                                                                    provider.Resolve<IRaceStartListService>());
            RaceDisplayViewModel = new RaceDisplayViewModel(RaceState);

            SetupCommands();
        }

        private void SetupCommands()
        {
            RaceBaseDataViewModel.OnUnsavedCancel += () => OnDelete?.Invoke(this);
            DeleteCommand = new RelayCommand(() => OnDelete?.Invoke(this),
                                             () => RaceState.Race.Id !=
                                                   -1); // && Race.RaceStateId == (int) Constants.RaceState.Upcoming
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