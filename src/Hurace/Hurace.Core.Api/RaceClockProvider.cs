using System;
using Hurace.Core.Timer;

namespace Hurace.Core.Api
{
    public class RaceClockProvider
    {
        public event Action<IRaceClock> OnRaceClockChanged;
        
        private static readonly Lazy<RaceClockProvider> LazyRaceClock =
            new Lazy<RaceClockProvider>(() => new RaceClockProvider());

        public static RaceClockProvider Instance => LazyRaceClock.Value;
        
        private IRaceClock _raceClock;
        public IRaceClock RaceClock => _raceClock;

        public void RegisterRaceClock(IRaceClock raceClock)
        {
            OnRaceClockChanged?.Invoke(raceClock);
            _raceClock = raceClock;
        }
    }
}