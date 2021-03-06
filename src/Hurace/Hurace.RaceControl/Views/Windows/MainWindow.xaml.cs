﻿using System.Windows;
using Autofac;
using Hurace.Core.Interface;
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
            var container = ((App)Application.Current).Container;
            var s = container.Resolve<ISkierService>();
            var vm = container.Resolve<MainWindowViewModel>();
            DataContext = vm;
            Loaded += async (sender, args) => await vm.InitializeAsync();
            InitializeComponent();
        }
    }
}