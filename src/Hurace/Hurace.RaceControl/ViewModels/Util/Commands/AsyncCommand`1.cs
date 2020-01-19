using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hurace.RaceControl.ViewModels.Commands
{
    public class AsyncCommand<T> : ICommand
    {
        private readonly Func<T, Task> _executeAsync;

        private readonly Predicate<T> _canExecute;
        
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public AsyncCommand(Func<T, Task> executeAsync, Predicate<T> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public async void Execute(object parameter) => await _executeAsync((T)parameter);
        public bool CanExecute(object parameter)
        {
            if (parameter is T p1) return _canExecute?.Invoke(p1) ?? true;
            return false;
        }

        public static void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}