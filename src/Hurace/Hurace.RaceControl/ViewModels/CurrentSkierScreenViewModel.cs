using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Api.Models;
using Hurace.Core.Api.RaceControlService.Service;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.Util;
using Hurace.RaceControl.ViewModels.ViewModelInterfaces;

namespace Hurace.RaceControl.ViewModels
{
    public class CurrentSkierScreenViewModel : ICurrentSkierVm, ISplitTimeVm
    {
        private IActiveRaceControlService _activeRaceControlService;
        private IRaceService _raceService;

        public StartList CurrentSkier { get; set; }
        public int? Position { get; set; }
        public TimeSpan? RaceTime { get; set; }
        public RaceStopwatch Stopwatch { get; set; }

        public ObservableCollection<TimeDifference> SplitTimeList { get; set; } = new ObservableCollection<TimeDifference>();
        
        public CurrentSkierScreenViewModel(IRaceService raceService, IActiveRaceControlService activeRaceControlService)
        {
            _raceService = raceService;
            _activeRaceControlService = activeRaceControlService;

            _activeRaceControlService.OnSplitTime += data => { };
            _activeRaceControlService.OnSkierFinished += startList => { };
            _activeRaceControlService.OnSkierStarted += startList => { };
            _activeRaceControlService.OnCurrentSkierDisqualified += startList => { };
        }

        public async Task InitializeAsync()
        {
            try
            {
                CurrentSkier = await _activeRaceControlService.GetCurrentSkier();
                if (CurrentSkier != null)
                {
                    if (SplitTimeList.Count > 1)
                        Position = await _activeRaceControlService.GetPossiblePositionForCurrentSkier();
                    Stopwatch.StartTime =
                        await _raceService.GetStartTimeForSkier(CurrentSkier.SkierId, CurrentSkier.RaceId);
                }
                else
                {
                    Position = null;
                    Stopwatch = null;
                }
            }
            catch (Exception)
            {
                ErrorNotifier.OnLoadError();
            }
        }


        
    }
}