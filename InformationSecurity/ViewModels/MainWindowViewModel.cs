using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using InformationSecurity.Infrastructure.Commands;
using InformationSecurity.Infrastructure.Encryptors;
using InformationSecurity.Views.Windows;

namespace InformationSecurity.ViewModels
{
    /// <summary>
    /// MainWindowViewModel class
    /// </summary>
    internal class MainWindowViewModel : ViewModelBase
    {

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

        #region Ключ шифрования / расшифрования

        /// <summary>
        /// Key to encryption field
        /// </summary>
        private string _encryptionKey = string.Empty;

        /// <summary>
        /// Key to encryption property
        /// </summary>
        public string EncryptionKey 
        { 
            get => _encryptionKey; 
            set 
            {
                Set(ref _encryptionKey, value);
                
                ClearErrors(nameof(EncryptionKey));

                ValidateKey();
            } 
        }

        #endregion

        #region Алгоритм шифрования / расшифрования

        /// <summary>
        /// Encryption (decryption) alg field
        /// </summary>
        private string _encryptionAlg;

        /// <summary>
        /// Encryption (decryption) alg property
        /// </summary>
        public string EncryptionAlg 
        { 
            get => _encryptionAlg;
            set 
            {
                Set(ref _encryptionAlg, value);

                ClearErrors(nameof(EncryptionKey));

                ValidateKey();
            }  
        
        }

        #endregion

        #region Валидация

        /// <summary>
        /// Key validation method
        /// </summary>
        public void ValidateKey()
        {
            if (EncryptionAlg == "Метод Цезаря" &&
                !string.IsNullOrEmpty(EncryptionKey) &&
                !int.TryParse(EncryptionKey, out _))
            {
                AddError(nameof(EncryptionKey), "Ключ должен быть целочисленным");
            }
        }

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
                    TextToDecryption = StringEncryptor
                        .GetCeaserEncryptedString(TextToEncryption, int.Parse(EncryptionKey), false);

                    break;
                case "Метод Виженера":
                    TextToDecryption = StringEncryptor
                        .GetVigenerEncyptedString(TextToEncryption, EncryptionKey, false);
                    break;
                default:
                    throw new ArgumentException("Unknown Encryption alg");
            
            }
        }

        /// <summary>
        /// Can EncryptCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanEncryptCommandExecute(object p)
        {
            if (!string.IsNullOrEmpty(TextToEncryption) &&
               !string.IsNullOrEmpty(EncryptionAlg) &&
               !string.IsNullOrEmpty(EncryptionKey) &&
               !HasErrors)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region DecryptCommand

        /// <summary>
        /// DecryptCommand field
        /// </summary>
        public ICommand DecryptCommand { get; set; }

        /// <summary>
        /// DecryptCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnDecryptCommandExecuted(object p)
        {
            switch (EncryptionAlg)
            {
                case "Метод Цезаря":
                    TextToEncryption = StringEncryptor
                        .GetCeaserEncryptedString(TextToDecryption, int.Parse(EncryptionKey), true);
                    break;
                case "Метод Виженера":
                    TextToEncryption = StringEncryptor
                        .GetVigenerEncyptedString(TextToDecryption, EncryptionKey, true);
                    break;
                default:
                    throw new ArgumentException("Unknown Decryption alg");

            }
        }

        /// <summary>
        /// Can DecryptCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanDecryptCommandExecute(object p)
        {
            if (!string.IsNullOrEmpty(TextToDecryption) &&
               !string.IsNullOrEmpty(EncryptionAlg) &&
               !string.IsNullOrEmpty(EncryptionKey) &&
               !HasErrors)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region DownloadCommand

        /// <summary>
        /// DownloadCommand field
        /// </summary>
        public ICommand DownloadCommand { get; }

        /// <summary>
        /// DownloadCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnDownloadCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Загрузить текстовый файл",
                Filter = "Текстовый файл (*.txt)|*.txt"
            };

            bool? response = openFileDialog.ShowDialog();

            if (response == false)
            {
                return;
            }

            try
            {
                using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
                {
                    TextToEncryption = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Can DownloadCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanDownloadCommandExecute(object p) => true;

        #endregion

        #region SaveCommand

        /// <summary>
        /// SaveCommand field
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// SaveCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnSaveCommandExecuted(object p)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Сохранить результат",
                Filter = "Текстовый файл (*.txt)|*.txt",
                AddExtension = true
            };

            bool? response = saveFileDialog.ShowDialog();

            if (response == false)
            {
                return;
            }

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    streamWriter.Write(TextToDecryption);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Can SaveCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanSaveCommandExecute(object p)
        {
            if (!string.IsNullOrEmpty(TextToDecryption))
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

        #region OpenVigenerTableCommand

        /// <summary>
        /// OpenVigenerTableCommand field
        /// </summary>
        public ICommand OpenVigenerTableCommand { get; }

        /// <summary>
        /// OpenVigenerTableCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnOpenVigenerTableCommandExecuted(object p)
        {
            VigenerTableWindow vigenerTableWindow = new VigenerTableWindow();
            vigenerTableWindow.Show();
        }

        /// <summary>
        /// Can OpenVigenerTableCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanOpenVigenerTableCommandExecute(object p) => true;

        #endregion

        #endregion

        /// <summary>
        /// MainWindowViewModel constructor
        /// </summary>
        public MainWindowViewModel()
        {
            #region Инициализация команд
            
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted,
                                                       CanCloseApplicationCommandExecute);

            SelectEncryptionAlgCommand = new RelayCommand(OnSelectEncryptionAlgCommandExecuted,
                                                            CanSelectEncryptionAlgCommandExecute);

            EncryptCommand = new RelayCommand(OnEncryptCommandExecuted, 
                                              CanEncryptCommandExecute);

            DecryptCommand = new RelayCommand(OnDecryptCommandExecuted, 
                                              CanDecryptCommandExecute);

            DownloadCommand = new RelayCommand(OnDownloadCommandExecuted, 
                                               CanDownloadCommandExecute);

            SaveCommand = new RelayCommand(OnSaveCommandExecuted, 
                                           CanSaveCommandExecute);

            OpenVigenerTableCommand = new RelayCommand(OnOpenVigenerTableCommandExecuted, 
                                                       CanOpenVigenerTableCommandExecute);

            #endregion
        }
    }
}
