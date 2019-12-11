using System;

namespace Hurace.RaceControl.ViewModels.Commands
{
    public class ActionCommand : BaseCommand
    {
        private readonly Action<object> _execute;
        
        public ActionCommand(Action<object> execute, Predicate<object> canExecute = null) : base(canExecute) => 
            _execute = execute ?? throw new ArgumentException();

        public override void Execute(object parameter) => _execute(parameter);
    }
}