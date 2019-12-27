using System;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Autofac;
using Autofac.Configuration;
using Hurace.Core.Logic.Modules;
using Hurace.RaceControl.ViewModels.SharedViewModels;
using Hurace.RaceControl.ViewModels.Util;
using Microsoft.Extensions.Configuration;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static IContainer SetupDependencyInjection()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationModule(config.Build()));
            var vmAssembly = Assembly.Load("Hurace.RaceControl");
            builder.RegisterAssemblyTypes(vmAssembly)
                   .Where(t => t.Name.EndsWith("ViewModel") &&
                               !Attribute.IsDefined(t, typeof(SingleInstanceAttribute)))
                   .AsSelf();
            builder.RegisterType<SharedRaceViewModel>().AsSelf().SingleInstance();
            return builder.Build();
        }

        public IContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Container ??= SetupDependencyInjection();
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
        }
    }
}