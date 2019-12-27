using System.Windows;
using Autofac;
using Autofac.Configuration;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Modules;
// using Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver;
using Hurace.RaceControl.ViewModels.Util;
using Hurace.RaceControl.ViewModels.WindowViewModels;
using Microsoft.Extensions.Configuration;

namespace Hurace.RaceControl.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            var container = ((App) Application.Current).Container;
            var vm = container.Resolve<MainViewModel>();
            DataContext = vm;
            // Loaded += async (sender, args) =>
            // {
            //     vm.
            //     if (!await container.Resolve<IActiveRaceResolver>().InitializeActiveRaceHandler()) ErrorNotifier.OnLoadError();
            // };
            
            InitializeComponent();
        }
    }
}