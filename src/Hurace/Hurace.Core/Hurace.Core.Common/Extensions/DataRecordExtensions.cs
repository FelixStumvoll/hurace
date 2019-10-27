using System;
using System.Data;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Common.Extensions
{
    public static class DataRecordExtensions
    {
        public static T MapTo<T>(this IDataRecord record, MapperConfig config = null)
            where T : class, new()
        {
            var ret = new T();
            foreach (var pi in typeof(T).GetProperties())
            {
                if (config != null && Attribute.IsDefined(pi, typeof(NavigationalAttribute)) &&
                    !config.exclusions.Contains(typeof(T)))
                {
                    var res = typeof(DataRecordExtensions).GetMethod(nameof(MapTo))?.MakeGenericMethod(pi.PropertyType)
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