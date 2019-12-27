﻿using System;
using System.Reflection;
using Autofac;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Resolver;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Interface;
using Microsoft.Extensions.Configuration;

namespace Hurace.Core.Logic
{
    // public sealed class ServiceProvider : IServiceProvider
    // {
    //     private IContainer? _container;
    //
    //     // private static readonly Lazy<ServiceProvider> Lazy =
    //     //     new Lazy<ServiceProvider>(() => new ServiceProvider());
    //
    //     //public static ServiceProvider Instance => Lazy.Value;
    //
    //     private ServiceProvider()
    //     {
    //         var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    //         BuildContainer(config, "hurace");
    //     }
    //
    //     private void BuildContainer(IConfiguration config, string configName)
    //     {
    //         var builder = new ContainerBuilder();
    //
    //         //Load Config 
    //         builder.Register(ctx => config).As<IConfiguration>().SingleInstance();
    //
    //         //Load Daos
    //         builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Dal.Dao"))
    //                .Where(t => t.Name.EndsWith("Dao") && (!t.Namespace?.Contains("Base") ?? false))
    //                .As(t => t.GetInterface($"I{t.Name}"));
    //
    //         //Load Services
    //         builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Core.Logic"))
    //                .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
    //
    //         builder.RegisterType<ActiveRaceResolver>().As<IActiveRaceResolver>();
    //
    //         //Load StatementFactory & ConnectionFactory
    //         var connectionStringSection = config.GetSection("ConnectionStrings").GetSection(configName);
    //         builder.RegisterInstance(
    //                    new ConcreteConnectionFactory(
    //                        DbUtil.GetProviderFactory(connectionStringSection["ProviderName"]),
    //                        connectionStringSection["ConnectionString"]))
    //                .As<IConnectionFactory>();
    //         builder.RegisterInstance(new StatementFactory("hurace")).AsSelf();
    //         builder.RegisterType<RaceClockProvider>().As<IRaceClockProvider>().SingleInstance();
    //         _container = builder.Build();
    //     }
    //
    //     public T? Resolve<T>() where T : class => _container?.Resolve<T>();
    // }
}