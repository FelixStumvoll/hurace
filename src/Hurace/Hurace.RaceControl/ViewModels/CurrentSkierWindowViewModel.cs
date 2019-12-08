using Hurace.Core.Api.RaceControl;
using Hurace.Core.Api.RaceCrud;
using Hurace.Dal.Domain;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class CurrentSkierWindowViewModel : NotifyPropertyChanged
    {
        private int _raceId;
        private IRaceControlService _raceControlService;
        private IRaceService _raceService;
        private StartList _currentSkier;
        private string _message;

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

        public CurrentSkierWindowViewModel(int raceId, IRaceService raceService)
        {
            _raceId = raceId;
            _raceService = raceService;
            _raceControlService = ActiveRaceHandler.Instance[raceId];
            _raceControlService.OnSkierFinished += OnSkierFinished;
            _raceControlService.OnSkierStarted += OnSkierStarted;
            _raceControlService.OnCurrentSkierDisqualified += OnCurrentSkierDisqualified;
        }

        private void OnSkierFinished(StartList startList)
        {
        }

        private void OnSkierStarted(StartList startList)
        {
            
        }

        private void OnCurrentSkierDisqualified(StartList startList)
        {
            
        }
    }
}