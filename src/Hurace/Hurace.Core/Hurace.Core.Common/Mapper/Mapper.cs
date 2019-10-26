using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Common.Mapper
{
    public class Mapper
    {
        private Dictionary<Type, Dictionary<string, string>> _mappingConfig = new
            Dictionary<Type, Dictionary<string, string>>();

        private bool loadNavigationals;

        public Mapper(bool loadNavigationals = true)
        {
            this.loadNavigationals = loadNavigationals;
        }

        public Mapper AddMapping<T>(params (string srcName, string destName)[] config) where T : new()
        {
            if (!_mappingConfig.TryGetValue(typeof(T), out var configDict))
                _mappingConfig[typeof(T)] = (configDict = new Dictionary<string, string>());
            config.ToList().ForEach(cfg => configDict.Add(cfg.destName, cfg.srcName));
            return this;
        }

        public T Map<T>(IDataRecord record) where T : new()
        {
            var ret = new T();
            foreach (var pi in typeof(T).GetProperties())
            {
                if (loadNavigationals && Attribute.IsDefined(pi, typeof(NavigationalAttribute)))
                {
                    var res = typeof(Mapper).GetMethod(nameof(Map))?.MakeGenericMethod(pi.PropertyType)
                        .Invoke(this, new object[] {record});
                    pi.SetValue(ret, res);
                    continue;
                }

                var propName = pi.Name.ToLowerFirstChar();
                if (_mappingConfig.TryGetValue(typeof(T), out var configDict) &&
                    configDict.TryGetValue(propName, out var srcName))
                    propName = srcName;

                pi.SetValue(ret, record[propName]);
            }

            return ret;
        }
    }
}