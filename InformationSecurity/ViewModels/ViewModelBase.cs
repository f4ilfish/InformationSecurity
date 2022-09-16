﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InformationSecurity.ViewModels
{
    /// <summary>
    /// ViewModelBase class
    /// </summary>
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event field
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Property changed method (Invoke / Raise)
        /// </summary>
        /// <param name="PropertyName">Property Name</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
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
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
