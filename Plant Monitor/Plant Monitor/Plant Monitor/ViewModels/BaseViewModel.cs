using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plant_Monitor.Models;
using Plant_Monitor.Services;
using Xamarin.Forms;

namespace Plant_Monitor.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        //Creating the variables
        public IDataStore<Plant> DataStore => DependencyService.Get<IDataStore<Plant>>();

        bool isBusy = false;

        //Creating the get and set functions for IsBusy
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        //Creating the get and set functions for Title
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        //Creating the function that will set the property
        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        //Creating the ProperyChanged variable
        public event PropertyChangedEventHandler PropertyChanged;
        //Creating the OnProperyChanged function
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            //Checking to see if the variables was changed
            if (changed == null)
                return;
            //Invokes if the variable was changed
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

}
