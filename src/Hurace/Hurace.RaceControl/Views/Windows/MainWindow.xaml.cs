using System.Windows;
using Autofac;
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
            var container = ((App)Application.Current).Container;
            var vm = container.Resolve<MainViewModel>();
            DataContext = vm;

            InitializeComponent();
        }
    }
}