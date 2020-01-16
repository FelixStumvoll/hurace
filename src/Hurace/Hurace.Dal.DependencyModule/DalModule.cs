using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Module = Autofac.Module;

namespace Hurace.Dal.DependencyModule
{
    [ExcludeFromCodeCoverage]
    public class DalModule : Module
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Dal.Dao"))
                   .Where(t => t.Name.EndsWith("Dao") && (!t.Namespace?.Contains("Base") ?? false))
                   .As(t => t.GetInterface($"I{t.Name}"));

            builder.RegisterInstance(
                       new ConcreteConnectionFactory(DbUtil.GetProviderFactory(ProviderName), ConnectionString))
                   .As<IConnectionFactory>();
            builder.RegisterInstance(new StatementFactory("hurace")).AsSelf();
        }
    }
}