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
            SaveFileDialog saveFileDialog = new()
            {
                Title = "Сохранить Таблицу Виженера",
                Filter = "Текстовый файл (*.txt)|*.txt",
                AddExtension = true
            };

            bool? response = saveFileDialog.ShowDialog();

            if (response == false) return;

            try
            {
                using StreamWriter streamWriter = new(saveFileDialog.FileName);
                foreach (var row in VigenerTable)
                {
                    streamWriter.Write(row);
                    streamWriter.Write('\n');
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// CanSaveVigenerTable execute method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanSaveVigenerTableCommandExecute(object p) => true;

        #endregion

        #region VigenerTable

        /// <summary>
        /// VigenerTable field
        /// </summary>
        private List<string> _vigenerTable;

        /// <summary>
        /// VigenerTable property
        /// </summary>
        public List<string> VigenerTable { get => _vigenerTable; set => Set(ref _vigenerTable, value); }

        #endregion

        /// <summary>
        /// VigenerTableViewModel constructor
        /// </summary>
        public VigenerTableViewModel()
        {
            VigenerTable = VigenerTableCreator.GetVigenerTable();

            SaveVigenerTableCommand = new RelayCommand(OnSaveVigenerTableExecuted, 
                                                       CanSaveVigenerTableCommandExecute);
        }
    }
}
