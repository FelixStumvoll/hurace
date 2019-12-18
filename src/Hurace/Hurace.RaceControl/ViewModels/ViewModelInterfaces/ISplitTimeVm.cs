using System.Collections.ObjectModel;
using Hurace.Core.Api.Models;
using Hurace.Dal.Domain;

namespace Hurace.RaceControl.ViewModels.ViewModelInterfaces
{
    public interface ISplitTimeVm
    {
        public ObservableCollection<TimeDifference> SplitTimeList { get; set; }
    }
}