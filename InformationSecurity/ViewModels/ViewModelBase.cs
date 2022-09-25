using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace InformationSecurity.ViewModels
{
    /// <summary>
    /// ViewModelBase class
    /// </summary>
    internal abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region INotifyPropertyChanged
        
        /// <summary>
        /// Property changed event field
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed method (Invoke / Raise)
        /// </summary>
        /// <param name="PropertyName">Property Name</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Refresh property value method (cyclically)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">Properties field reference</param>
        /// <param name="value">New properties field value</param>
        /// <param name="PropertyName">Property name</param>
        /// <returns>Is properties field value refresh</returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        #endregion

        #region IDisposable

        //~ViewModelBase()
        //{
        //    Dispose(false);
        //}

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _Disposed;

        /// <summary>
        /// Dispose method (releasing managed resources)
        /// </summary>
        /// <param name="Disposing"></param>
        protected virtual void Dispose(bool Disposing)
        {
            if (!Disposing || _Disposed) return;
            _Disposed = true;
        }

        #endregion
    }
}
