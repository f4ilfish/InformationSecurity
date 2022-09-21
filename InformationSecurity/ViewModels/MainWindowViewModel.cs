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
        private string _title = "Information Security";

        /// <summary>
        /// Title property
        /// </summary>
        public string Title { get => _title; set => Set(ref _title, value); }
        #endregion

        #region Статус
        /// <summary>
        /// Status field
        /// </summary>
        private string _status = "Initialize";

        /// <summary>
        /// Status property
        /// </summary>
        public string Status { get => _status; set => Set(ref _status, value); }
        #endregion

        #region Текст для шифрования

        /// <summary>
        /// Text to encryption field
        /// </summary>
        private string _textToEncryption = string.Empty;

        /// <summary>
        /// Text to enccryption property
        /// </summary>
        public string TextToEncryption { get => _textToEncryption; set => Set(ref _textToEncryption, value); }

        #endregion

        #region Текст для расшифрования

        /// <summary>
        /// Text to dencryption field
        /// </summary>
        private string _textToDecryption = string.Empty;

        /// <summary>
        /// Text to denccryption property
        /// </summary>
        public string TextToDecryption { get => _textToDecryption; set => Set(ref _textToDecryption, value); }

        #endregion

        #region Ключь шифрования

        /// <summary>
        /// Key to encryption field
        /// </summary>
        private string _encryptionKey = string.Empty;

        /// <summary>
        /// Key to encryption property
        /// </summary>
        public string EncryptionKey { get => _encryptionKey; set => Set(ref _encryptionKey, value); }

        #endregion

        #region Алгоритм шифрования

        private string _encryptionAlg;

        public string EncryptionAlg { get => _encryptionAlg; set => Set(ref _encryptionAlg, value); }

        #endregion

        #region Команды

        #region SelectEncryptionAlgCommand

        /// <summary>
        /// SelectEncryptionAlgCommand field
        /// </summary>
        public ICommand SelectEncryptionAlgCommand { get; set; }

        /// <summary>
        /// SelectEncryptionAlgCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnSelectEncryptionAlgCommandExecuted(object p)
        {
            EncryptionAlg = (string)p;
        }

        /// <summary>
        /// Can SelectEncryptionAlgCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanSelectEncryptionAlgCommandExecute(object p) => true;

        #endregion

        #region EncryptCommand

        /// <summary>
        /// EncryptCommand field
        /// </summary>
        public ICommand EncryptCommand { get; set; }

        /// <summary>
        /// EncryptCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnEncryptCommandExecuted(object p)
        {
            switch (EncryptionAlg)
            {
                case "Метод Цезаря":
                    TextToDecryption = InformationSecurityConsoleTest.StringEncryptor
                        .GetCeaserEncryptedString(TextToEncryption, int.Parse(EncryptionKey), false);
                    break;
                case "Метод Виженера":
                    TextToDecryption = InformationSecurityConsoleTest.StringEncryptor
                        .GetVigenerEncyptedString(TextToEncryption, EncryptionKey, false);
                    break;
                default:
                    throw new ArgumentException("Unknown Encryption alg");
            
            }
        }

        /// <summary>
        /// Can CloseApplicationCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanEncryptCommandExecute(object p)
        {
            if (!string.IsNullOrEmpty(TextToEncryption) &&
               !string.IsNullOrEmpty(EncryptionAlg) &&
               !string.IsNullOrEmpty(EncryptionKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion

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

            SelectEncryptionAlgCommand = new RelayCommand(OnSelectEncryptionAlgCommandExecuted,
                                                            CanSelectEncryptionAlgCommandExecute);

            EncryptCommand = new RelayCommand(OnEncryptCommandExecuted, 
                                              CanEncryptCommandExecute);

            #endregion
        }
    }
}
