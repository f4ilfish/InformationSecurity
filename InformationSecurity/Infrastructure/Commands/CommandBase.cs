using System;
using System.Windows.Input;

namespace InformationSecurity.Infrastructure.Commands
{
    /// <summary>
    /// CommandBase class
    /// </summary>
    internal abstract class CommandBase: ICommand
    {
        /// <summary>
        /// CanExecuteChanged event
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// CanExecute command method
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Execute command method
        /// </summary>
        /// <param name="parameter"></param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void Execute(object parameter);
    }
}
