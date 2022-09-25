using System;
using System.Windows;
using System.Windows.Input;
using InformationSecurity.Infrastructure.Commands;
using InformationSecurity.Infrastructure.Encryptors;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.IO;

namespace InformationSecurity.ViewModels
{
    /// <summary>
    /// MainWindowViewModel class
    /// </summary>
    internal class MainWindowViewModel : ViewModelBase, INotifyDataErrorInfo
    {               
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

                if (EncryptionAlg == "Метод Цезаря" && !int.TryParse(_encryptionKey, out _))
                {
                    AddError(nameof(EncryptionKey), "Ключ должен быть целочисленным");
                }
            } 
        }

        #endregion

        #region Алгоритм шифрования / расшифрования

        private string _encryptionAlg;

        public string EncryptionAlg 
        { 
            get => _encryptionAlg;
            set 
            {
                Set(ref _encryptionAlg, value);

                ClearErrors(nameof(EncryptionKey));

                OnPropertyChanged(nameof(EncryptionKey));
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

        #endregion

        /// <summary>
        /// MainWindowViewModel constructor
        /// </summary>
        public MainWindowViewModel()
        {
            #region Команды
            
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

            #endregion
        }

        #region Валидация

        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public bool HasErrors => _propertyErrors.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyErrors.GetValueOrDefault(propertyName, null);
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }

            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ClearErrors(string propertyName)
        {
            if (_propertyErrors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }

        #endregion
    }
}
