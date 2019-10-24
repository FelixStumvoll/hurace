using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dto;

namespace Hurace.TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Expression<Func<IDataReader, Country>> countryMapper = src => new Country
            {
                Id = (int) src["countryId"], Name = (string) src["countryName"]
            };
            var mapperconfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDataReader, Country>();
                cfg.CreateMap<IDataReader, Skier>().ForMember(s => s.Country,
                                                              opt => opt.MapFrom(countryMapper));
            });
            var mapper = mapperconfig.CreateMapper();
            var providername = "Microsoft.Data.SqlClient";
            var cstring = "Data Source=localhost;Initial Catalog=testdb;Integrated Security=True";
            var skierDao =
                new SkierDao(
                    new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providername), cstring, providername),
                    mapper, "hurace.skier");
            var res = await skierDao.FindAllAsync();
            Console.WriteLine("Hello World!");
        }
    }
}