﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurace.Core.Common.Mapper
{
    public class MapperConfig
    {
        public readonly Dictionary<Type, Dictionary<string, string>> MappingConfig = new
            Dictionary<Type, Dictionary<string, string>>();

        public readonly HashSet<Type> Exclusions = new HashSet<Type>();
        private readonly HashSet<Type> _inclusions = new HashSet<Type>();

        public MapperConfig AddMapping<T>(params (string srcName, string destName)[] config) where T : class, new()
        {
            if (!MappingConfig.TryGetValue(typeof(T), out var configDict))
                MappingConfig[typeof(T)] = (configDict = new Dictionary<string, string>());
            config.ToList().ForEach(cfg => configDict.Add(cfg.destName, cfg.srcName));
            return this;
        }

        public MapperConfig AddExclusion<T>() where T : class, new()
        {
            Exclusions.Add(typeof(T));
            return this;
        }

        public MapperConfig Include<T>() where T : class, new()
        {
            _inclusions.Add(typeof(T));
            return this;
        }

        public bool IsIncluded(Type t) => _inclusions.Contains(t);

        public bool MappingExists(Type t, string propName, out string srcName)
        {
            if (MappingConfig.TryGetValue(t, out var propertyMappings) &&
                propertyMappings.TryGetValue(propName, out srcName)) return true;
            srcName = default;
            return false;
        }
    }
}