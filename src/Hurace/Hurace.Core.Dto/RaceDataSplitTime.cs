
using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class RaceDataSplitTime : RaceData
    {
        [Navigational]
        public TimeData? TimeData { get; set; }
    }
}