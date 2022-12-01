using System;
using System.Windows.Input;

namespace OnlineClicker_bot
{
    internal class RelayICommand : ICommand
    {
        public RelayICommand(Action<object> execute) : this(null, execute)
        { }

        public RelayICommand(Predicate<object> canExecute, Action<object> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;

            return canExecute(parameter);
        }

        public void Execute(object parameter) => execute(parameter);
    }
}