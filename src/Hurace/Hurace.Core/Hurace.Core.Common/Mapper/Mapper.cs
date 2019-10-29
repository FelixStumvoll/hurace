﻿using System;
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
                if (Attribute.IsDefined(pi, typeof(NavigationalAttribute)))
                {
                    if (config?.IsIncluded(pi.PropertyType) ?? false)
                    {
                        var res = typeof(Mapper).GetMethod(nameof(MapTo))?.MakeGenericMethod(pi.PropertyType)
                            .Invoke(null, new object[] {record, config});
                        pi.SetValue(ret, res);
                    }
                    continue;
                }

                string mappedName = null;
                var propName = config?.MappingExists(typeof(T), pi.Name, out mappedName) ?? false
                    ? mappedName
                    : pi.Name.ToLowerFirstChar();
                    pi.SetValue(ret, record[propName]);
            }

            return ret;
        }
    }
}