using System;
using System.Windows.Input;

namespace Hurace.RaceControl.ViewModels.Commands
{
    public abstract class BaseCommand : ICommand
    {
        private protected readonly Predicate<object> _canExecute;

        protected BaseCommand(Predicate<object> canExecute)
        {
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}