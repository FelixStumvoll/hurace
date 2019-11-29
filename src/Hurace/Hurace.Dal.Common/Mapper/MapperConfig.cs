using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurace.Dal.Common.Mapper
{
    public class MapperConfig
    {
        private readonly Dictionary<Type, Dictionary<string, string>> _mappingConfig = new
            Dictionary<Type, Dictionary<string, string>>();
        
        private readonly HashSet<Type> _inclusions = new HashSet<Type>();

        public MapperConfig AddMapping<T>(params (string srcName, string destName)[] config) where T : class, new()
        {
            if (!_mappingConfig.TryGetValue(typeof(T), out var configDict))
                _mappingConfig[typeof(T)] = (configDict = new Dictionary<string, string>());
            config.ToList().ForEach(cfg =>
            {
                var (srcName, destName) = cfg;
                if(!configDict.ContainsKey(destName)) configDict.Add(destName, srcName);
            });
            _inclusions.Add(typeof(T));
            return this;
        }
        
        public MapperConfig Include<T>() where T : class, new()
        {
            _inclusions.Add(typeof(T));
            return this;
        }

        public bool IsIncluded(Type t) => _inclusions.Contains(t);

        public bool MappingExists(Type t, string propName, out string? srcName)
        {
            if (_mappingConfig.TryGetValue(t, out var propertyMappings) &&
                propertyMappings.TryGetValue(propName, out srcName)) return true;
            srcName = default;
            return false;
        }
    }
}