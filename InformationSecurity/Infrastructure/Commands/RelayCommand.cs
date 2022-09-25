using System;

namespace InformationSecurity.Infrastructure.Commands
{
    /// <summary>
    /// RelayCommand class
    /// </summary>
    internal class RelayCommand : CommandBase
    {
        /// <summary>
        /// Object delegate (action) field
        /// </summary>
        private readonly Action<object> _execute;
        
        /// <summary>
        /// Object to bool func field
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// RelayCommand constructor
        /// </summary>
        /// <param name="Execute"></param>
        /// <param name="CanExecute"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute = null) 
        {
            _execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _canExecute = CanExecute;
        }

        /// <inheritdoc/>
        public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        /// <inheritdoc/>
        public override void Execute(object parameter) => _execute(parameter);
    }
}
