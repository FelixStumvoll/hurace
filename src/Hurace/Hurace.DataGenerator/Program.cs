using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Hurace.DataGenerator
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static async Task Main()
        {
            var dbCreator = new DbDataCreator("Microsoft.Data.SqlClient",
                                              "Data Source=localhost;Initial Catalog=huraceDB_Prod;Persist Security Info=True;User ID=SA;Password=EHq(iT|$@A4q");
            
            await dbCreator.Cleanup();
            try
            {
                await dbCreator.FillDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await dbCreator.Cleanup();
            }

        }
    }
}