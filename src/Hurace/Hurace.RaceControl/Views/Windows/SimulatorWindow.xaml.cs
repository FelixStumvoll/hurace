using Hurace.RaceControl.ViewModels.WindowViewModels;

namespace Hurace.RaceControl.Views.Windows
{
    public partial class SimulatorWindow
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