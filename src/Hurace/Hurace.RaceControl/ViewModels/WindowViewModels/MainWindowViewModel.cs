using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.PageViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.WindowViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private IPageViewModel _currentPage;
        private bool _backVisible;
        private MainPageViewModel _mainPageViewModel;
        public ICommand BackToMainCommand { get; set; }

        public bool BackVisible
        {
            get => _backVisible;
            set => Set(ref _backVisible, value);
        }

        public IPageViewModel CurrentPage
        {
            get => _currentPage;
            set => Set(ref _currentPage, value, true);
        }

        public MainViewModel()
        {
            _mainPageViewModel = new MainPageViewModel(ChangePage);
            CurrentPage = _mainPageViewModel;
            BackToMainCommand = new AsyncCommand(async _ => await ChangePage(_mainPageViewModel));
        }

        private async Task ChangePage(IPageViewModel vm)
        {
            BackVisible = vm != _mainPageViewModel;
            await vm.SetupAsync();
            CurrentPage = vm;
        }
    }
}