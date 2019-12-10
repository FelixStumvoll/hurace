using System;
using System.Reflection;
using System.Threading.Tasks;
using Hurace.Core.Timer;
using Microsoft.Extensions.Configuration;

namespace Hurace.Core.Api
{
    public class RaceClockProvider
    {
        private static readonly Lazy<RaceClockProvider> LazyRaceClock =
            new Lazy<RaceClockProvider>(() => new RaceClockProvider());

        public static RaceClockProvider Instance => LazyRaceClock.Value;

        private IRaceClock _raceClock;

        public async Task<IRaceClock> GetRaceClock()
        {
            if (_raceClock == null)
                await Task.Run(() =>
                {
                    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var clockSection = config.GetSection("Clock");
                    var type = Assembly.Load(clockSection["Assembly"])
                                       .GetType($"{clockSection["Assembly"]}.{clockSection["ClassName"]}");
                    if (type.GetInterface(nameof(IRaceClock)) == null) return;
                    _raceClock = (IRaceClock) Activator.CreateInstance(type);
                });
            return _raceClock;
        }
    }
}