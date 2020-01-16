using System.Collections.Generic;

namespace Hurace.Core.Interface.Configs
{
    public class SensorConfig
    {
        public int MaxDiffToAverage { get; }
        public List<int> SensorAssumptions { get; }

        public SensorConfig(int maxDiffToAverage, List<int> sensorAssumptions)
        {
            MaxDiffToAverage = maxDiffToAverage;
            SensorAssumptions = sensorAssumptions;
        }
    }
}