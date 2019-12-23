using System.Threading.Tasks;

namespace Hurace.RaceControl.ViewModels.RaceControlViewModels
{
    public class ReadonlyRaceControlViewModel : IRaceControlViewModel
    {
        public Task SetupAsync()
        {
            return Task.CompletedTask;
        }
    }
}