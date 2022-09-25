using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.Win32;
using InformationSecurity.Infrastructure.Encryptors;
using InformationSecurity.Infrastructure.Commands;

namespace InformationSecurity.ViewModels
{
    /// <summary>
    /// VigenerTableViewModel class
    /// </summary>
    internal class VigenerTableViewModel : ViewModelBase
    {
        #region SaveVigenerTableCommand

        /// <summary>
        /// SaveVigenerTable field
        /// </summary>
        public ICommand SaveVigenerTableCommand { get; }

        /// <summary>
        /// SaveVigenerTable execute method
        /// </summary>
        /// <param name="p"></param>
        private void OnSaveVigenerTableExecuted(object p)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Сохранить Таблицу Виженера",
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
                    foreach (var row in VigenerTable)
                    {
                        streamWriter.Write(row);
                        streamWriter.Write('\n');
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Can SaveVigenerTable execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanSaveVigenerTableCommandExecute(object p)
        {
            if (VigenerTable.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Таблица Виженера

        /// <summary>
        /// Vigener Table field
        /// </summary>
        private List<string> _vigenerTable;

        /// <summary>
        /// Vigener Table property
        /// </summary>
        public List<string> VigenerTable { get => _vigenerTable; set => Set(ref _vigenerTable, value); }

        #endregion

        /// <summary>
        /// VigenerTableViewModel constructor
        /// </summary>
        public VigenerTableViewModel()
        {
            #region Обсчет таблицы Виженера

            string encryptionAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                                      "abcdifghijklmnopqrstuvwxyz" +
                                                      "АБВГДЕЖЗИЙКЛМНОПРСТФХЦЧШЩЪЫЬЭЮЯ" +
                                                      "абвгдежзиклмнопрстуфхцчшщъыьэюя" +
                                                      "0123456789";

            var vigenerTable = new List<string>();
            vigenerTable.Add(encryptionAlphabet);

            foreach (char ch in encryptionAlphabet)
            {
                var vigenerTableRow = string.Empty;
                vigenerTableRow += ch;

                foreach (char keyCh in encryptionAlphabet)
                {
                    vigenerTableRow += CharEncryptor.GetVigenerEncryptedChar(keyCh, ch, false);
                }

                vigenerTable.Add(vigenerTableRow);
            }

            VigenerTable = vigenerTable;

            #endregion

            SaveVigenerTableCommand = new RelayCommand(OnSaveVigenerTableExecuted, 
                                                       CanSaveVigenerTableCommandExecute);
        }
    }
}
