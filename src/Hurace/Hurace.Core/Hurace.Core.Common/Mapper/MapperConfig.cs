using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurace.Core.Common.Mapper
{
    public class MapperConfig
    {
        public readonly Dictionary<Type, Dictionary<string, string>> MappingConfig = new
            Dictionary<Type, Dictionary<string, string>>();
        
        
        public MapperConfig AddMapping<T>(params (string srcName, string destName)[] config) where T : new()
        {
            if (!MappingConfig.TryGetValue(typeof(T), out var configDict))
                MappingConfig[typeof(T)] = (configDict = new Dictionary<string, string>());
            config.ToList().ForEach(cfg => configDict.Add(cfg.destName, cfg.srcName));
            return this;
        }
        
    }
}