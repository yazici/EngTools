namespace Pamux.GameDev.UserControls.Commands
{
    using Pamux.GameDev.UserControls.MVVM;
    using System;
    using System.Windows.Input;

    public abstract class CommandBase : ICommand
    {
        public CommandBase()
        {
        }

        #region ICommand Members

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            var vm = parameter as IPamuxViewModel;
            if (vm != null)
            {
                Execute(vm.M, vm.V, vm);
            }
        }

        public abstract void Execute(IPamuxModel m, IPamuxView v, IPamuxViewModel vm);

        #endregion
    }
}
