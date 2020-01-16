using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using Hurace.Core.Interface;
using Hurace.Core.Service;
using Module = Autofac.Module;

namespace Hurace.Core.DependencyModule
{
    [ExcludeFromCodeCoverage]
    public class CoreServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Load Services
            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Core.Service"))
                   .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();

            builder.RegisterType<ActiveRaceResolver>().As<IActiveRaceResolver>()
                   .OnActivating(async handler => await handler.Instance.InitializeActiveRaceResolver())
                   .SingleInstance();
            
            //Load StatementFactory & ConnectionFactory
            builder.RegisterType<RaceClockProvider>().As<IRaceClockProvider>().SingleInstance();
        }
    }
}