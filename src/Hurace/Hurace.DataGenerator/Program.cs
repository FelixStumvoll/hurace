using System;
using System.Threading.Tasks;

namespace Hurace.DataGenerator
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            await new DbDataCreator("Microsoft.Data.SqlClient", "Data Source=huracedbserver.database.windows.net;Initial Catalog=huraceDB;Persist Security Info=True;User ID=FelixStumvoll;Password=EHq(iT|$@A4q").Run();
        }
    }
}