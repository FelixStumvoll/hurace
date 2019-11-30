using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Hurace.Core.Api.Race;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Dao;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Hurace.Dal.Interface.Base;
using Microsoft.Extensions.Configuration;

namespace Hurace.Core.Api
{
    public static class ContainerFactory
    {
        public static IContainer BuildContainer(IConfiguration config, string configName)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Dal.Dao"))
                   .Where(t => t.Name.EndsWith("Dao") && (!t.Namespace?.Contains("Base") ?? false))
                   .As(t => t.GetInterface($"I{t.Name}"));

            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Core.Api"))
                   .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            
            var section = config.GetSection("ConnectionStrings").GetSection(configName);
            
            builder.RegisterInstance(
                       new ConcreteConnectionFactory(DbUtil.GetProviderFactory(section["ProviderName"]),
                                                     section["ConnectionString"]))
                   .As<IConnectionFactory>();
            builder.RegisterInstance(new StatementFactory("hurace")).AsSelf();
            return builder.Build();
        }
    }
}