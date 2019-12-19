using System.Windows;
using Hurace.Core.Api.ActiveRaceControlService.Resolver;
using Hurace.RaceControl.ViewModels.Util;
using Hurace.RaceControl.ViewModels.WindowViewModels;

namespace Hurace.RaceControl.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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