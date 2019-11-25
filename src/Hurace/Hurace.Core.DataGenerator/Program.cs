﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Hurace.DataGenerator
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static async Task Main()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var section = config.GetSection("ConnectionStrings").GetSection("huraceProd");
            var dbCreator = new DbDataCreator(
                section["ProviderName"],
                section["ConnectionString"]);

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