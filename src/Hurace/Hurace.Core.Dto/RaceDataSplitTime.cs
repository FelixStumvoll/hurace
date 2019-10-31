using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class RaceDataSplitTime : RaceData
    {
        [Navigational]
        public TimeData? TimeData { get; set; }
    }
}