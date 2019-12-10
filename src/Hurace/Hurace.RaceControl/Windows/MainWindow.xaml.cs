using System.Windows;
using Hurace.Core.Api.RaceControl;
using Hurace.RaceControl.ViewModels;
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
            ActiveRaceHandler.InitializeActiveRaceHandler();
            var vm = new MainViewModel();
            DataContext = vm;
            InitializeComponent();
        }
    }
}