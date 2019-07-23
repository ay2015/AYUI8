namespace Ay.MvcFramework
{
    using System;
    using System.Windows.Input;

    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter) == false)
                return;

            OnExecute(parameter);
        }

        protected abstract void OnExecute(object parameter);

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
