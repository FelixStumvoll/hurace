using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hurace.RaceControl.ViewModels.Commands
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;

        private readonly Func<bool> _canExecute;
        
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public AsyncCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public async void Execute(object parameter) => await _executeAsync();
        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public static void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}