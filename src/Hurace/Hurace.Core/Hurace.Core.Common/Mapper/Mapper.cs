using System;
using System.Data;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Common.Mapper
{
    public static class Mapper
    {
        public static T MapTo<T>(IDataRecord record, MapperConfig config = null)
            where T : class, new()
        {
            var ret = new T();
            foreach (var pi in typeof(T).GetProperties())
            {
                if (config != null && Attribute.IsDefined(pi, typeof(NavigationalAttribute)) &&
                    !config.Exclusions.Contains(typeof(T)))
                {
                    var res = typeof(Mapper).GetMethod(nameof(MapTo))?.MakeGenericMethod(pi.PropertyType)
                        .Invoke(null, new object[] {record, config});
                    pi.SetValue(ret, res);
                    continue;
                }

                var propName = pi.Name.ToLowerFirstChar();
                if (config != null && config.MappingConfig.TryGetValue(typeof(T), out var configDict) &&
                    configDict.TryGetValue(propName, out var srcName))
                    propName = srcName;

                pi.SetValue(ret, record[propName]);
            }

            return ret;
        }
    }
}