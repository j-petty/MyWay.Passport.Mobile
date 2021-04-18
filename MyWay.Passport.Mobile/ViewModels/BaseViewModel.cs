﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Variables
        /// <summary>
        /// Required in ViewModels to control page navigation.
        /// </summary>
        private INavigation navigation;
        public INavigation Navigation
        {
            get { return navigation; }
            set { SetProperty(ref navigation, value); }
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        #endregion

        public BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
