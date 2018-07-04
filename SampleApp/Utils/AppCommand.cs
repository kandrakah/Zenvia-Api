using System;
using System.Windows.Input;

namespace SampleApp.Utils
{
    public class AppCommand : ICommand
    {
        public bool Enabled { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        private Action<object> execute;

        private Predicate<object> canExecute;

        public AppCommand(Action<object> execute) : this(execute, null) { }

        public AppCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.Enabled = true;
            this.execute = execute ?? throw new ArgumentNullException("Execução nula!");
            this.canExecute = canExecute ?? DefaultCanExecute;
        }
        
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter) && Enabled;
        }

        private bool DefaultCanExecute(object parameter)
        {
            return Enabled;
        }
    }
}
