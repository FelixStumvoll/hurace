using System.Windows;
using Autofac;
using Hurace.RaceControl.ViewModels.WindowViewModels;

namespace Hurace.RaceControl.Views.Windows
{
    public partial class SimulatorWindow
    {
        public SimulatorWindow()
        {
            var container = ((App)Application.Current).Container;
            var vm = container.Resolve<SimulationWindowViewModel>();
            DataContext = vm;
            Loaded += async (sender, args) => await vm.InitializeAsync();
            InitializeComponent();
            Closed += (sender, args) => vm.OnClose();
        }
    }
}