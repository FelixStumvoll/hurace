using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dto;

namespace Hurace.TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var providername = "Microsoft.Data.SqlClient";
            var cstring = "Data Source=localhost;Initial Catalog=huraceDB;Persist Security Info=True;User ID=SA;Password=EHq(iT|$@A4q";
            var skierDao =
                new SkierDao(
                    new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providername), cstring, providername));
            var res = await skierDao.FindAllAsync();
            Console.WriteLine("Hello World!");
        }
    }
}