using System;
using System.Collections.ObjectModel;
using Hurace.Core.Api.ActiveRaceControlService.Resolver;
using Hurace.Core.Api.ActiveRaceControlService.Service;
using Hurace.Core.Api.RaceService;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.BaseViewModels;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels.WindowViewModels
{
    public class CurrentSkierWindowViewModel : NotifyPropertyChanged
    {
        private int _raceId;
        private readonly IActiveRaceControlService _activeRaceControlService;
        private IRaceService _raceService;
        private StartList _currentSkier;
        private string _message;

        public ObservableCollection<TimeDataComparison> TimeDataComparisons { get; set; } =
            new ObservableCollection<TimeDataComparison>();

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public StartList CurrentSkier
        {
            get => _currentSkier;
            set => Set(ref _currentSkier, value);
        }

        public class TimeDataComparison
        {
            public TimeData TimeData { get; set; }
            public TimeSpan DiffToLeader { get; set; }
        }

        public CurrentSkierWindowViewModel(int raceId, IRaceService raceService)
        {
            _raceId = raceId;
            _raceService = raceService;
            _activeRaceControlService = ActiveRaceResolver.Instance[raceId];
            _activeRaceControlService.OnSkierFinished += OnSkierFinished;
            _activeRaceControlService.OnSkierStarted += OnSkierStarted;
            _activeRaceControlService.OnCurrentSkierDisqualified += OnCurrentSkierDisqualified;
            _activeRaceControlService.OnSplitTime += OnSplitTime;
        }

        private void OnSkierFinished(StartList startList)
        {
        }

        private void OnSkierStarted(StartList startList)
        {
            CurrentSkier = startList;
            Message = "";
            TimeDataComparisons.Clear();
        }

        private void OnCurrentSkierDisqualified(StartList startList)
        {
        }

        private void OnSplitTime(TimeData timeData)
        {
            
        }
    }
}