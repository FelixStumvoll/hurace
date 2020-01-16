using System;
using System.Reflection;
using System.Threading.Tasks;
using Core.Common.Configs;
using Hurace.Core.Interface;
using Hurace.Core.Timer;
using ClockConfig = Hurace.Core.Interface.Configs.ClockConfig;

namespace Hurace.Core.Service
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