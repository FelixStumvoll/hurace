﻿using System.Windows;
using Hurace.Core.Api;
using Hurace.Core.Api.RaceService;
using Hurace.RaceControl.ViewModels.WindowViewModels;

namespace Hurace.RaceControl.Windows
{
    public partial class CurrentSkierWindow : Window
    {
        public CurrentSkierWindow(int raceId)
        {
            var vm = new CurrentSkierWindowViewModel(raceId, ServiceProvider.Instance.ResolveService<IRaceService>());
            DataContext = vm;
            InitializeComponent();
        }
    }
}