using System.Windows;
using Hurace.RaceControl.ViewModels.WindowViewModels;

namespace Hurace.RaceControl.Windows
{
    public partial class SimulatorWindow : Window
    {
        public SimulatorWindow()
        {
            var vm = new SimulationWindowViewModel();
            DataContext = vm;
            Loaded += async (sender, args) => await vm.InitializeAsync();
            InitializeComponent();
        }
    }
}