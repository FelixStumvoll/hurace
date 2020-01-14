﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Hurace.Core.Logic.Configs;

namespace Hurace.Core.Logic.Modules
{
    [ExcludeFromCodeCoverage]
    public class CoreConfigModule : Module
    {
        public string ClockAssembly { get; set; }
        public string ClockClassName { get; set; }
        public int MaxDiffToAverage { get; set; }
        public List<int> SensorAssumptions { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new ClockConfig(ClockAssembly, ClockClassName)).AsSelf()
                   .SingleInstance();
            builder.RegisterInstance(new SensorConfig(MaxDiffToAverage, SensorAssumptions)).AsSelf()
                   .SingleInstance();
        }
    }
}