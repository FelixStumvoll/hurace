using System;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.ViewModelInterfaces
{
    public interface ICurrentSkierVm
    {
        public StartList CurrentSkier { get; set; }
        public int? Position { get; set; }
        public TimeSpan? RaceTime { get; set; }
    }
}