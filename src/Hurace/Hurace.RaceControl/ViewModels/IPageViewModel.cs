using System.Threading.Tasks;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public interface IPageViewModel
    {
        Task SetupAsync();
    }
}