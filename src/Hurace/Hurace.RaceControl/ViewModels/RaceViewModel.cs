using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Core.Api;
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

        public event Action<RaceViewModel> OnDelete;

        public RaceViewModel(IRaceService logic, Race race, SharedRaceViewModel svm)
        {
            Race = race;
            RaceStartListViewModel = new RaceStartListViewModel(logic, race);
            RaceBaseDataViewModel = new RaceBaseDataViewModel(logic, race, svm);
            RaceControlViewModel = new RaceControlViewModel();
            RaceBaseDataViewModel.OnUnsavedCancel += () => OnDelete?.Invoke(this);
            DeleteCommand = new ActionCommand(_ => OnDelete?.Invoke(this), _ => Race.Id != -1);
        }

        public async Task SetupAsync()
        {
            await RaceStartListViewModel.SetupAsync();
            await Task.Run(() =>
            {
                Task.Delay(500);
                RaceBaseDataViewModel.Setup();
            });
        }
    }
}