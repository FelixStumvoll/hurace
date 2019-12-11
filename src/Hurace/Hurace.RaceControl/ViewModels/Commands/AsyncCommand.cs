using System;
using System.Threading.Tasks;

namespace Hurace.RaceControl.ViewModels.Commands
{
    public class AsyncCommand : BaseCommand
    {
        private readonly Func<object, Task> _executeAsync;

        public AsyncCommand(Func<object, Task> executeAsync, Predicate<object> canExecute = null) : base(canExecute) =>
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));

        public override async void Execute(object parameter) => await _executeAsync(parameter);
    }
}