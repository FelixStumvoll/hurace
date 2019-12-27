using System.Collections.Generic;

namespace Hurace.Core.Logic.Configs
{
    public interface ISensorConfig
    {
        int MaxDiffToAverage { get; }
        List<int> SensorAssumptions { get; }
    }
}