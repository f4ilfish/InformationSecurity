using System.Windows;

namespace InformationSecurity.Infrastructure.Commands
{
    /// <summary>
    /// CloseApplicationCommand class
    /// </summary>
    internal class CloseApplicationCommand : CommandBase
    {
        /// <inheritdoc/>
        public override bool CanExecute(object parameter) => true;

        /// <summary>
        /// Close Application method
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
