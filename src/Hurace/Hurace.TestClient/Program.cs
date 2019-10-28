using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;

namespace Hurace.TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("YETR");
            var res = QueryBuilder.SelectAll<TimeData>("hurace").Join<TimeData,Skier>(("skierId", "id")).Join<Skier, Country>(("countryId", "id")).Build();
            var rx = QueryBuilder.Update<Skier>("hurace").Where(("Id", 1)).Build(new Skier());

            //
//
// var providername = "Microsoft.Data.SqlClient";
//            var cstring = "Data Source=huracedbserver.database.windows.net;Initial Catalog=huraceDB;Persist Security Info=True;User ID=FelixStumvoll;Password=EHq(iT|$@A4q";
//            var skierDao =
//                new SkierDao(
//                    new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providername), cstring, providername));
//            var res = await skierDao.FindAllAsync();
//            Console.WriteLine("Hello World!");
        }
    }
}