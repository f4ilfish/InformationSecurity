using System;
using System.Windows;
using System.Windows.Input;
using InformationSecurity.Infrastructure.Commands;

namespace InformationSecurity.ViewModels
{
    /// <summary>
    /// MainWindowViewModel class
    /// </summary>
    internal class MainWindowViewModel : ViewModelBase
    {
        /// набор свойств для элементов
                
        #region Заголовок окна
        /// <summary>
        /// Title field
        /// </summary>
        private string _title;

        /// <summary>
        /// Title property
        /// </summary>
        public string Title { get => _title; set => Set(ref _title, value); }
        #endregion

        #region Статус
        /// <summary>
        /// Status field
        /// </summary>
        private string _status;

        /// <summary>
        /// Status property
        /// </summary>
        public string Status { get => _status; set => Set(ref _status, value); }
        #endregion

        #region Команды
        
        #region CloseApplicationCommand
        
        /// <summary>
        /// CloseApplicationCommand field
        /// </summary>
        public ICommand CloseApplicationCommand { get; }

        /// <summary>
        /// CloseApplicationCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Can CloseApplicationCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanCloseApplicationCommandExecute(object p) => true;
        
        #endregion
        
        #endregion


        public MainWindowViewModel()
        {
            #region Команды
            
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted,
                                                       CanCloseApplicationCommandExecute);
            
            #endregion
        }
    }
}
