using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Plant_Monitor.Models;
using Plant_Monitor.Views;
using Xamarin.Forms;
using Npgsql;

namespace Plant_Monitor.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
  //Creating the variables
        private Plant _selectedItem;

        public ObservableCollection<Plant> Plants { get; }
        public Command RefreshCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Plant> ItemTapped { get; }

        public ItemsViewModel()
        {
            //Sets the Title variable
            Title = "Active Plant List";
            //Sets the Plants variables
            Plants = new ObservableCollection<Plant>();
            //Sets the LoadItemsCommand variable
            //RefreshCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //sets the ItemTapped variable
            ItemTapped = new Command<Plant>(OnItemSelected);
            //Sets the AddItemCommand variable
            AddItemCommand = new Command(OnAddItem);
        }
        //Creates the OnAppering function
        public void OnAppearing()
        {
            //Tell the code you are doing a command
            IsBusy = true;
            //Set the items to null
            SelectedItem = null;
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


        //Creating the OnAddItem function
        private async void OnAddItem(object obj)
        {
            //Navigates to the new itme page
            await Shell.Current.GoToAsync(nameof(NewItemPage));
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
