using System;
using System.Data;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Common.Extensions
{
    public static class DataRecordExtensions
    {
        public static T MapTo<T>(this IDataRecord record, MapperConfig config = null)
            where T : class, new() =>
            Mapper.Mapper.MapTo<T>(record, config);
    }
}