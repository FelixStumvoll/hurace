using System.Data;
using Hurace.Dal.Common.Mapper;

namespace Hurace.Dal.Common.Extensions
{
    public static class DataRecordExtensions
    {
        public static T MapTo<T>(this IDataRecord record, MapperConfig? config)
            where T : class, new() =>
            Mapper.Mapper.MapTo<T>(record, config);
    }
}