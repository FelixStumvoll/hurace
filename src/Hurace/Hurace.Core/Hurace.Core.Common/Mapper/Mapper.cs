﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Hurace.Core.Common.Extensions;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Common.Mapper
{
    public static class Mapper
    {
        public static T Map<T>(IDataRecord record, MapperConfig config = null, bool mapNavigational = true)
            where T : new()
        {
            var ret = new T();
            foreach (var pi in typeof(T).GetProperties())
            {
                if (mapNavigational && Attribute.IsDefined(pi, typeof(NavigationalAttribute)))
                {
                    var res = typeof(Mapper).GetMethod(nameof(Map))?.MakeGenericMethod(pi.PropertyType)
                        .Invoke(null, new object[] {record, config, mapNavigational});
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