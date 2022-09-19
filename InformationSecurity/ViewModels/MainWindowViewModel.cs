using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSecurity.ViewModels
{
    /// <summary>
    /// MainWindowViewModel class
    /// </summary>
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Заголовок окна
        /// <summary>
        /// Title field
        /// </summary>
        private string _title;

        /// <summary>
        /// Title property
        /// </summary>
        public string Title 
        {
            get => _title;
            set => Set(ref _title, value); 
        }
        #endregion
    }
}
