using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.PageViewModels;

namespace Hurace.RaceControl.ViewModels.WindowViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private IPage _currentPage;
        private bool _backVisible;
        private readonly MainPageViewModel _mainPageViewModel;
        public ICommand BackToMainCommand { get; set; }

        public bool BackVisible
        {
            get => _backVisible;
            set => Set(ref _backVisible, value);
        }

        public IPage CurrentPage
        {
            get => _currentPage;
            set => Set(ref _currentPage, value, true);
        }

        public MainViewModel(Func<Func<IPage, Task>, MainPageViewModel> mainPageVmFactory)
        {
            _mainPageViewModel = mainPageVmFactory(ChangePage);
            CurrentPage = _mainPageViewModel;
            BackToMainCommand = new AsyncCommand(async () => await ChangePage(_mainPageViewModel));
        }

        private async Task ChangePage(IPage vm)
        {
            BackVisible = vm != _mainPageViewModel;
            await vm.SetupAsync();
            CurrentPage = vm;
        }
    }
}