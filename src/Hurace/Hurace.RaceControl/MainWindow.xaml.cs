using System.Windows;
using System.Windows.Navigation;
using Hurace.Core.Api.RaceControl;
using Hurace.RaceControl.ViewModels;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            //var provider = ServiceProvider.Instance;
            ActiveRaceHandler.InitializeActiveRaceHandler();
            var vm = new MainViewModel(); //provider.ResolveService<IRaceService>()
            DataContext = vm;
            InitializeComponent();
            //Loaded += async (sender, args) => await vm.SetupAsync();
        }
    }
}