using Hurace.Dal.Domain;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceControlViewModel
    {
        public Race Race { get; set; }

        public RaceControlViewModel(Race race)
        {
            Race = race;
        }
    }
}