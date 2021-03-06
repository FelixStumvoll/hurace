﻿namespace Hurace.Core.Interface.Configs
{
    public class ClockConfig 
    {
        public string ClockAssembly { get; }
        public string ClockClassName { get; }
        
        public ClockConfig(string clockAssembly, string clockClassName)
        {
            ClockAssembly = clockAssembly;
            ClockClassName = clockClassName;
        }
    }
}