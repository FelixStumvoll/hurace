using System;
using System.Reflection;
using Autofac;
using Hurace.Core.Api.Race;
using Hurace.Dal;

namespace Hurace.Core.Api
{
    public class ServiceFactory
    {
        public ServiceFactory()
        {
            var builder = new ContainerBuilder();
            var a = Assembly.Load("Hurace.Dal.Dao");
            builder.RegisterAssemblyTypes(Assembly.Load("Hurace.Dal.Dao"))
                   .Where(t => t.Name.EndsWith("Dao")).AsImplementedInterfaces();
        }
        
        public IRaceService CreateRaceService()
        {
            return null;
        }
    }
}