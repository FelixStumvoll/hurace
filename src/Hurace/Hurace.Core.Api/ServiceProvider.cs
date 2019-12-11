using System;
using System.Reflection;
using Autofac;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Microsoft.Extensions.Configuration;

namespace Hurace.Core.Api
{
    public sealed class ServiceProvider
    {
        private IContainer _container;

        private static readonly Lazy<ServiceProvider> Lazy = new Lazy<ServiceProvider>(() => new ServiceProvider());
        
        public static ServiceProvider Instance => Lazy.Value;
        
        private ServiceProvider()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            BuildContainer(config, "hurace");
        }

        private void BuildContainer(IConfiguration config, string configName)
        {
            var builder = new ContainerBuilder();
            
            //Load Daos
            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Dal.Dao"))
                   .Where(t => t.Name.EndsWith("Dao") && (!t.Namespace?.Contains("Base") ?? false))
                   .As(t => t.GetInterface($"I{t.Name}"));

            //Load Services
            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Core.Api"))
                   .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();

            //Load StatementFactory & ConnectionFactory
            var connectionStringSection = config.GetSection("ConnectionStrings").GetSection(configName);
            builder.RegisterInstance(
                       new ConcreteConnectionFactory(DbUtil.GetProviderFactory(connectionStringSection["ProviderName"]),
                                                     connectionStringSection["ConnectionString"]))
                   .As<IConnectionFactory>();
            builder.RegisterInstance(new StatementFactory("hurace")).AsSelf();
            _container = builder.Build();
        }

        public T ResolveService<T>() => _container.Resolve<T>();
    }
}