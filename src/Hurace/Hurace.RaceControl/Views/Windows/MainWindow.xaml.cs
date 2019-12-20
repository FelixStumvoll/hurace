using Hurace.Core.Logic.ActiveRaceControlService.Resolver;
using Hurace.RaceControl.ViewModels.Util;
using Hurace.RaceControl.ViewModels.WindowViewModels;

namespace Hurace.RaceControl.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            var vm = new MainViewModel();
            DataContext = vm;
            Loaded += async (sender, args) =>
            {
                if (!await ActiveRaceResolver.InitializeActiveRaceHandler()) ErrorNotifier.OnLoadError();
            };
            
            InitializeComponent();
        }
    }
}