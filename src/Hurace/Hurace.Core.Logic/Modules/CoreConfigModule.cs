using System.Collections.Generic;
using Autofac;
using Hurace.Core.Logic.Configs;

namespace Hurace.Core.Logic.Modules
{
    public class CoreConfigModule : Module
    {
        public string ClockAssembly { get; set; }
        public string ClockClassName { get; set; }
        public int MaxDiffToAverage { get; set; }
        public List<int> SensorAssumptions { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new ClockConfig(ClockAssembly, ClockClassName)).As<IClockConfig>()
                   .SingleInstance();
            builder.RegisterInstance(new SensorConfig(MaxDiffToAverage, SensorAssumptions)).As<ISensorConfig>()
                   .SingleInstance();
        }
    }
}