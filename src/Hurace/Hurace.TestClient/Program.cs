using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dto;

namespace Hurace.TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var res = new QueryFactory("hurace").Select<Skier>().Join<Skier, Country>(("countryId", "id"))
                .Where<Skier>(("id", 1)).Build();
//            var res = QueryBuilder.SelectAll<TimeData>("hurace").Join<TimeData,Skier>(("skierId", "id")).Join<Skier, Country>(("countryId", "id")).Build();


            //
//
            var providername = "Microsoft.Data.SqlClient";
            var cstring =
                "Data Source=huracedbserver.database.windows.net;Initial Catalog=huraceDB;Persist Security Info=True;User ID=FelixStumvoll;Password=EHq(iT|$@A4q";
            var skierDao =
                new RaceDao(
                    new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providername), cstring, providername),
                    new QueryFactory("hurace"));
            var resx = await skierDao.GetStartList(1);
//            Console.WriteLine("Hello World!");
        }
    }
}