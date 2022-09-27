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
        #region Шифрование

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

        #region ValidateKey

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

        #endregion

        #region Обмен ключами

        #region p

        /// <summary>
        /// g field
        /// </summary>
        private string _p = string.Empty;

        /// <summary>
        /// P property
        /// </summary>
        public string P
        {
            get => _p;
            set
            {
                Set(ref _p, value);

                ClearErrors(nameof(P));

                if (!string.IsNullOrEmpty(P) &&
                    !int.TryParse(P, out _))
                {
                    AddError(nameof(P), "P должен быть целочисленным");
                }
                
                if(int.TryParse(P, out var n))
                {
                    if (!(n > 1)) AddError(nameof(P), "P должен быть больше 1");

                    for (int i = 2; i < n; i++)
                    {
                        if (n % i == 0)
                        {
                            AddError(nameof(P), "P не является простым числом");
                        }
                    }
                }
            }
        }

        #endregion

        #region g

        /// <summary>
        /// g field
        /// </summary>
        private string _g = string.Empty;

        /// <summary>
        /// P property
        /// </summary>
        public string G
        {
            get => _g;
            set
            {
                Set(ref _g, value);

                ClearErrors(nameof(G));

                if (!string.IsNullOrEmpty(G) 
                    && !int.TryParse(G, out _))
                {
                    AddError(nameof(G), "G должен быть целочисленным");
                }

                if (int.TryParse(G, out var n))
                {
                    if (!(n > 1) || (n >= 10)) AddError(nameof(G), "G должен быть в диапазоне 1...9");

                    for (int i = 2; i < n; i++)
                    {
                        if (n % i == 0)
                        {
                            AddError(nameof(G), "G не является простым числом");
                        }
                    }
                }
            }
        }

        #endregion

        #region Текст Алисы

        /// <summary>
        /// Alice's text field
        /// </summary>
        private string _aliceText = string.Empty;

        /// <summary>
        /// Alice's text property
        /// </summary>
        public string AliceText { get => _aliceText; set => Set(ref _aliceText, value); }

        #endregion

        #region Текст Боба

        /// <summary>
        /// Bob's text field
        /// </summary>
        private string _bobText = string.Empty;

        /// <summary>
        /// Bob's text property
        /// </summary>
        public string BobText { get => _bobText; set => Set(ref _bobText, value); }

        #endregion

        #region RefreshCommand

        /// <summary>
        /// RefreshCommand field
        /// </summary>
        public ICommand RefreshCommand { get; set; }

        /// <summary>
        /// RefreshCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnRefreshCommandExecuted(object p)
        {
            P = string.Empty;
            G = string.Empty;
            AliceText = string.Empty;
            BobText = string.Empty;
        }

        /// <summary>
        /// Can RefreshCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanRefreshCommandExecute(object p)
        {
            if (!string.IsNullOrEmpty(P) 
                && !string.IsNullOrEmpty(G) 
                && !string.IsNullOrEmpty(AliceText)
                && !string.IsNullOrEmpty(BobText))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

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
               !HasErrorsByProperty(nameof(EncryptionKey)))
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

        #region KeyExchangeCommand

        /// <summary>
        /// KeyExchangeCommand field
        /// </summary>
        public ICommand KeyExchangeCommand { get; set; }

        /// <summary>
        /// KeyExchangeCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnKeyExchangeCommandExecuted(object p)
        {
            AliceText += $"Алиса и Боб договорились: p = {P}; g = {G}\n";
            BobText += $"Алиса и Боб договорились: p = {P}; g = {G}\n";

            Random rnd = new();
            var a = (double)rnd.Next(2, int.Parse(P));
            var b = (double)rnd.Next(2, int.Parse(P));

            AliceText += $"Алиса случайным образом выбрала: а = {a}\n";
            BobText += $"Боб случайным образом выбрал: b = {b}\n";

            var A = (Math.Pow(double.Parse(G), a) % double.Parse(P));
            var B = (Math.Pow(double.Parse(G), b) % double.Parse(P));

            AliceText += $"Алиса вычислила: А = g^(a) mod p = {A}\n";
            BobText += $"Боб получил от Алисы: А = {A}\n";

            BobText += $"Боб вычислил: B = g^(b) mod p = {B}\n";
            AliceText += $"Алиса получила от Боба: B = {B}\n";

            var kAlice = (Math.Pow(B, a) % double.Parse(P));
            var kBob = (Math.Pow(A, b) % double.Parse(P));

            AliceText += $"Алиса вычислила: K = B^a mod p = {kAlice}\n";
            BobText += $"Боб вычислил: K = A^b mod p = {kBob}\n";
        }

        /// <summary>
        /// Can KeyExchangeCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanKeyExchangeCommandExecute(object p)
        {
            if (!string.IsNullOrEmpty(P) 
                && !string.IsNullOrEmpty(G)
                && !HasErrorsByProperty(nameof(P))
                && !HasErrorsByProperty(nameof(G)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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

            KeyExchangeCommand = new RelayCommand(OnKeyExchangeCommandExecuted, CanKeyExchangeCommandExecute);

            RefreshCommand = new RelayCommand(OnRefreshCommandExecuted, CanRefreshCommandExecute);

            #endregion
        }
    }
}
