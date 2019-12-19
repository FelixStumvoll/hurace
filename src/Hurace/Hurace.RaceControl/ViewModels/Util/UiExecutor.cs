using System;
using System.Threading.Tasks;
using System.Windows;

namespace Hurace.RaceControl.ViewModels.Util
{
    public static class UiExecutor
    {
        public static Task ExecuteInUiThreadAsync(Func<Task> func) =>
            Application.Current.Dispatcher?.Invoke(async () => await func());

        public static void ExecuteInUiThreadAsync(Action func) =>
            Application.Current.Dispatcher?.Invoke(func);
    }
}