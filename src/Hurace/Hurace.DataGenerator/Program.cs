using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Hurace.DataGenerator
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var dbCreator = new DbDataCreator("Microsoft.Data.SqlClient",
                                              "Data Source=huracedbserver.database.windows.net;Initial Catalog=huraceDB;Persist Security Info=True;User ID=FelixStumvoll;Password=EHq(iT|$@A4q");
            try
            {
                //await dbCreator.FillDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
                await dbCreator.Cleanup();
            
        }
    }
}