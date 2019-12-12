using System.Threading.Tasks;

namespace Hurace.RaceControl.ViewModels.PageViewModels
{
    public interface IPageViewModel
    {
        Task SetupAsync();
    }
}