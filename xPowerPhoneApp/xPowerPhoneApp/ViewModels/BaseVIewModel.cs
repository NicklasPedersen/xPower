﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using xPowerPhoneApp.Interfaces;

namespace xPowerPhoneApp.ViewModels
{

    /// <summary>
    /// Base class for view models
    /// </summary>
    abstract internal class BaseViewModel : INotifyPropertyChanged
    {
        protected IChangePage _pageChanger;

        /// <summary>
        /// Base class for view models
        /// </summary>
        /// <param name="pageChanger">The object that can Change page</param>
        protected BaseViewModel(IChangePage pageChanger)
        {
            _pageChanger = pageChanger;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}