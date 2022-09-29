﻿using System;
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

        #region Text to encryption

        /// <summary>
        /// Text to encryption field
        /// </summary>
        private string _textToEncryption = string.Empty;

        /// <summary>
        /// Text to enccryption property
        /// </summary>
        public string TextToEncryption { get => _textToEncryption; set => Set(ref _textToEncryption, value); }

        #endregion

        #region Text to dencryption

        /// <summary>
        /// Text to dencryption field
        /// </summary>
        private string _textToDecryption = string.Empty;

        /// <summary>
        /// Text to denccryption property
        /// </summary>
        public string TextToDecryption { get => _textToDecryption; set => Set(ref _textToDecryption, value); }

        #endregion

        #region Key to encryption

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

        #region Encryption algorithm

        /// <summary>
        /// EncryptionAlgorithm field
        /// </summary>
        private string _encryptionAlgorithm;

        /// <summary>
        /// EncryptionAlgorithm property
        /// </summary>
        public string EncryptionAlgorithm 
        { 
            get => _encryptionAlgorithm;
            set 
            {
                Set(ref _encryptionAlgorithm, value);

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
            if (EncryptionAlgorithm == "Метод Цезаря" &&
                !string.IsNullOrEmpty(EncryptionKey) &&
                !int.TryParse(EncryptionKey, out _))
            {
                AddError(nameof(EncryptionKey), "Ключ должен быть целочисленным");
            }
        }

        #endregion

        #endregion

        #region Обмен ключами

        #region ModValue

        /// <summary>
        /// ModValue field
        /// </summary>
        private string _modValue = string.Empty;

        /// <summary>
        /// ModValue property
        /// </summary>
        public string ModValue
        {
            get => _modValue;
            set
            {
                Set(ref _modValue, value);

                ClearErrors(nameof(ModValue));

                if(int.TryParse(ModValue, out var n))
                {
                    if (!(n > 1)) AddError(nameof(ModValue), "Должно быть простым числом");
                    
                    for (int i = 2; i < n; i++)
                    {
                        if (n % i == 0) AddError(nameof(ModValue), "Должно быть простым числом");
                    }
                }
            }
        }

        #endregion

        #region PowBaseValue

        /// <summary>
        /// PowBaseValue field
        /// </summary>
        private string _powBaseValue = string.Empty;

        /// <summary>
        /// PowBaseValue property
        /// </summary>
        public string PowBaseValue
        {
            get => _powBaseValue;
            set
            {
                Set(ref _powBaseValue, value);

                ClearErrors(nameof(PowBaseValue));

                if (int.TryParse(PowBaseValue, out var n))
                {
                    if (!(n > 1) || n > 9) AddError(nameof(PowBaseValue), "Основание должно быть простым числом от 2 до 9");

                    for (int i = 2; i < n; i++)
                    {
                        if (n % i == 0) AddError(nameof(PowBaseValue), "Основание должно быть простым числом от 2 до 9");
                    }
                }
            }
        }

        #endregion

        #region Alice's text

        /// <summary>
        /// Alice's text field
        /// </summary>
        private string _aliceText = string.Empty;

        /// <summary>
        /// Alice's text property
        /// </summary>
        public string AliceText { get => _aliceText; set => Set(ref _aliceText, value); }

        #endregion

        #region Bob's text

        /// <summary>
        /// Bob's text field
        /// </summary>
        private string _bobText = string.Empty;

        /// <summary>
        /// Bob's text property
        /// </summary>
        public string BobText { get => _bobText; set => Set(ref _bobText, value); }

        #endregion

        #endregion

        #region Команды

        #region SelectEncryptionAlgCommand

        /// <summary>
        /// SelectEncryptionAlgorithmCommand field
        /// </summary>
        public ICommand SelectEncryptionAlgorithmCommand { get; set; }

        /// <summary>
        /// SelectEncryptionAlgorithmCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnSelectEncryptionAlgorithmCommandExecuted(object p)
        {
            EncryptionAlgorithm = (string)p;
        }

        /// <summary>
        /// CanSelectEncryptionAlgorithmCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanSelectEncryptionAlgorithmCommandExecute(object p) => true;

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
            switch (EncryptionAlgorithm)
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
            return !string.IsNullOrEmpty(TextToEncryption) &&
                   !string.IsNullOrEmpty(EncryptionAlgorithm) &&
                   !string.IsNullOrEmpty(EncryptionKey) &&
                   !HasErrorsByProperty(nameof(EncryptionKey));
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
            switch (EncryptionAlgorithm)
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
            return !string.IsNullOrEmpty(TextToDecryption) &&
                   !string.IsNullOrEmpty(EncryptionAlgorithm) &&
                   !string.IsNullOrEmpty(EncryptionKey) &&
                   !HasErrorsByProperty(nameof(EncryptionKey));
        }

        #endregion

        #region DownloadTextToEncryptCommand

        /// <summary>
        /// DownloadTextToEncryptCommand field
        /// </summary>
        public ICommand DownloadTextToEncryptCommand { get; }

        /// <summary>
        /// OnDownloadTextToEncryptCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnDownloadTextToEncryptCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Загрузить текстовый файл",
                Filter = "Текстовый файл (*.txt)|*.txt"
            };

            bool? response = openFileDialog.ShowDialog();

            if (response == false) return;

            try
            {
                using StreamReader streamReader = new(openFileDialog.FileName);
                TextToEncryption = streamReader.ReadToEnd();
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
        private bool CanDownloadTextToEncryptCommandExecute(object p) => true;

        #endregion

        #region SaveEncryptedTextCommand

        /// <summary>
        /// SaveEncryptedTextCommand field
        /// </summary>
        public ICommand SaveEncryptedTextCommand { get; }

        /// <summary>
        /// OnSaveEncryptedTextCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnSaveEncryptedTextCommandExecuted(object p)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Title = "Сохранить результат",
                Filter = "Текстовый файл (*.txt)|*.txt",
                AddExtension = true
            };

            bool? response = saveFileDialog.ShowDialog();

            if (response == false) return;

            try
            {
                using StreamWriter streamWriter = new(saveFileDialog.FileName);
                streamWriter.Write(TextToDecryption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// CanSaveEncryptedTextCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanSaveEncryptedTextCommandExecute(object p) => !string.IsNullOrEmpty(TextToDecryption);

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
            VigenerTableWindow vigenerTableWindow = new();
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
            AliceText += $"Алиса и Боб договорились: p = {ModValue}; g = {PowBaseValue}\n";
            BobText += $"Алиса и Боб договорились: p = {ModValue}; g = {PowBaseValue}\n";

            Random rnd = new();
            var a = (double)rnd.Next(2, int.Parse(ModValue));
            var b = (double)rnd.Next(2, int.Parse(ModValue));

            AliceText += $"Алиса случайным образом выбрала: а = {a}\n";
            BobText += $"Боб случайным образом выбрал: b = {b}\n";

            var A = (Math.Pow(double.Parse(PowBaseValue), a) % double.Parse(ModValue));
            var B = (Math.Pow(double.Parse(PowBaseValue), b) % double.Parse(ModValue));

            AliceText += $"Алиса вычислила: А = g^(a) mod p = {A}\n";
            BobText += $"Боб получил от Алисы: А = {A}\n";

            BobText += $"Боб вычислил: B = g^(b) mod p = {B}\n";
            AliceText += $"Алиса получила от Боба: B = {B}\n";

            var kAlice = (Math.Pow(B, a) % double.Parse(ModValue));
            var kBob = (Math.Pow(A, b) % double.Parse(ModValue));

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
            return !string.IsNullOrEmpty(ModValue) &&
                   !string.IsNullOrEmpty(PowBaseValue) &&
                   !HasErrorsByProperty(nameof(ModValue)) &&
                   !HasErrorsByProperty(nameof(PowBaseValue));
        }

        #endregion

        #region ResetKeyExchangeCommand

        /// <summary>
        /// ResetKeyExchangeCommand field
        /// </summary>
        public ICommand ResetKeyExchangeCommand { get; set; }

        /// <summary>
        /// RefreshCommand execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnResetKeyExchangeCommandExecuted(object p)
        {
            ModValue = string.Empty;
            PowBaseValue = string.Empty;
            AliceText = string.Empty;
            BobText = string.Empty;
        }

        /// <summary>
        /// CanRefreshCommand execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanResetKeyExchangeCommandExecute(object p) => true;

        #endregion

        #endregion

        /// <summary>
        /// MainWindowViewModel constructor
        /// </summary>
        public MainWindowViewModel()
        {
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted,
                                                       CanCloseApplicationCommandExecute);

            SelectEncryptionAlgorithmCommand = new RelayCommand(OnSelectEncryptionAlgorithmCommandExecuted,
                                                            CanSelectEncryptionAlgorithmCommandExecute);

            EncryptCommand = new RelayCommand(OnEncryptCommandExecuted, 
                                              CanEncryptCommandExecute);

            DecryptCommand = new RelayCommand(OnDecryptCommandExecuted, 
                                              CanDecryptCommandExecute);

            DownloadTextToEncryptCommand = new RelayCommand(OnDownloadTextToEncryptCommandExecuted, 
                                               CanDownloadTextToEncryptCommandExecute);

            SaveEncryptedTextCommand = new RelayCommand(OnSaveEncryptedTextCommandExecuted, 
                                           CanSaveEncryptedTextCommandExecute);

            OpenVigenerTableCommand = new RelayCommand(OnOpenVigenerTableCommandExecuted, 
                                                       CanOpenVigenerTableCommandExecute);

            KeyExchangeCommand = new RelayCommand(OnKeyExchangeCommandExecuted, 
                                                  CanKeyExchangeCommandExecute);

            ResetKeyExchangeCommand = new RelayCommand(OnResetKeyExchangeCommandExecuted, 
                                                       CanResetKeyExchangeCommandExecute);
        }
    }
}
