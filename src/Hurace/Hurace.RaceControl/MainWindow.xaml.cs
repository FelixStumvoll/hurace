using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using Hurace.Core.Api;
using Hurace.Core.Api.RaceControl;
using Hurace.Core.Api.RaceCrud;
using Hurace.RaceControl.ViewModels;
using Microsoft.Extensions.Configuration;
using ContainerBuilder = Autofac.ContainerBuilder;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var provider = ServiceProvider.Instance;
            var vm = new MainViewModel(provider.ResolveService<IRaceService>());
            DataContext = vm;
            InitializeComponent();
            Loaded += async (sender, args) => await vm.SetupAsync();
        }
    }
}