using System;
using System.Reflection;
using System.Threading.Tasks;
using Hurace.Core.Logic.Configs;
using Hurace.Core.Timer;
using Microsoft.Extensions.Configuration;

namespace Hurace.Core.Logic
{
    public class RaceClockProvider : IRaceClockProvider
    {
        private readonly ClockConfig _clockConfig;
        private IRaceClock? _raceClock;

        public RaceClockProvider(ClockConfig clockConfig)
        {
            _clockConfig = clockConfig;
        }

        public async Task<IRaceClock?> GetRaceClock()
        {
            if (_raceClock == null)
                await Task.Run(() =>
                {
                    var type = Assembly.Load(_clockConfig.ClockAssembly)
                                       .GetType($"{_clockConfig.ClockAssembly}.{_clockConfig.ClockClassName}");
                    if (type == null || type.GetInterface(nameof(IRaceClock)) == null) return;
                    _raceClock = (IRaceClock?) Activator.CreateInstance(type);
                });
            return _raceClock;
        }
    }
}