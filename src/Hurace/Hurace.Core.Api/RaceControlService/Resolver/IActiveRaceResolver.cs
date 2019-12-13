using System;
using System.Threading.Tasks;
using Hurace.Core.Api.RaceControlService.Service;
using Hurace.Core.Api.Util;

namespace Hurace.Core.Api.RaceControlService.Resolver
{
    public interface IActiveRaceResolver
    {
        Task<Result<IActiveRaceControlService, Exception>> StartRace(int raceId);
        IActiveRaceControlService this[int raceId] { get; }
        Task<Result<bool,Exception>> EndRace(int raceId);
    }
}