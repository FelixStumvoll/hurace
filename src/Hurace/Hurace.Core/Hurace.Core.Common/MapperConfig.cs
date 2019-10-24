using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Hurace.Core.Dto;

namespace Hurace.Core.Common
{
    public static class MapperConfig
    {
        private class DataReaderConverter<T> : ITypeConverter<IDataReader, T> where T : class, new()
        {
            public T Convert(IDataReader source, T destination, ResolutionContext context)
            {
                destination ??= new T();

                typeof(T).GetProperties().ToList().ForEach(p => p.SetValue(destination, source[p.Name]));
                return destination;
            }
        }

        private static T Convert<T>(IDataReader reader) where T : class, new()
        {
            var dest = new T();
            typeof(T).GetProperties().ToList().ForEach(p => p.SetValue(dest, reader[p.Name]));
            return dest;
        }

        public static IMapper GetMapper()
        {
            Expression<Func<IDataReader, Country>> countryMapper = src => new Country
            {
                Id = (int) src["CountryId"],
                Name = (string) src["CountryName"]
            };

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDataReader, Country>();
//                cfg.CreateMap<IDataReader, Skier>();
                cfg.CreateMap(typeof(IDataReader), typeof(Skier));
            });

            return mapperConfig.CreateMapper();
        }
    }
}