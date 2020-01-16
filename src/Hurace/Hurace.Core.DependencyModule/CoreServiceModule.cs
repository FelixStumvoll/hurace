using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using Hurace.Core.Interface;
using Hurace.Core.Service;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Module = Autofac.Module;

namespace Hurace.Core.DependencyModule
{
    [ExcludeFromCodeCoverage]
    public class CoreServiceModule : Module
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            //Load Daos
            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Dal.Dao"))
                   .Where(t => t.Name.EndsWith("Dao") && (!t.Namespace?.Contains("Base") ?? false))
                   .As(t => t.GetInterface($"I{t.Name}"));

            //Load Services
            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Core.Service"))
                   .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();

            builder.RegisterType<ActiveRaceResolver>().As<IActiveRaceResolver>()
                   .OnActivating(async handler => await handler.Instance.InitializeActiveRaceResolver())
                   .SingleInstance();

            //Load StatementFactory & ConnectionFactory
            builder.RegisterInstance(
                       new ConcreteConnectionFactory(DbUtil.GetProviderFactory(ProviderName), ConnectionString))
                   .As<IConnectionFactory>();
            builder.RegisterInstance(new StatementFactory("hurace")).AsSelf();
            builder.RegisterType<RaceClockProvider>().As<IRaceClockProvider>().SingleInstance();
        }
    }
}