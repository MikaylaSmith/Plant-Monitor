using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Plant_Monitor.Models;
using Plant_Monitor.Views;
using Plant_Monitor.ViewModels;
using Xamarin.Forms;

namespace Plant_Monitor.ViewModels
{
    public class ArchiveViewModel : BaseViewModel
    {
        //Creating the variables
        private Plant _selectedItem;

        public ObservableCollection<Plant> Plants { get; }
        public Command RefreshCommand { get; }
        public Command<Plant> ItemTapped { get; }

        public Command BackCommand { get; }

        //Creating the OnCancel function
        public ArchiveViewModel()
        {
            //Sets the Title variable
            Title = "Archive Plant List";
            //Sets the Plants variables
            Plants = new ObservableCollection<Plant>();
            //Sets the LoadItemsCommand variable
            //RefreshCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //sets the ItemTapped variable
            ItemTapped = new Command<Plant>(OnItemSelected);
            //Sets the AddItemCommand variable
            BackCommand = new Command(OnBack);
        }
        //Creates the OnAppering function
        public void OnAppearing()
        {
            //Tell the code you are doing a command
            IsBusy = true;
            //Set the items to null
            SelectedItem = null;
        }

        private async void OnBack()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        //Creating the get and set for SelectedItem
        public Plant SelectedItem
        {
            //Get function for selected item
            get => _selectedItem;
            //Set function for selected item
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }


        async void OnItemSelected(Plant plant)
        {
            if (plant == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.Common_Name)}={plant.CommonName}");
        }
    }
}
