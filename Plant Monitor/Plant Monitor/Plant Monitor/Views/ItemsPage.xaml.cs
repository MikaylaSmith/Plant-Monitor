using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plant_Monitor.Models;
using Plant_Monitor.ViewModels;
using Plant_Monitor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Npgsql;
using System.Windows.Input;
using System.Diagnostics;

namespace Plant_Monitor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ObservableCollection<Plant> plants;
        //Creating a variable for the content of the search filter
        //Sets the LoadItemsCommand variable
        private string searchFilterSelected { get; set; }
        //Creating a variable for the item view model
        //ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            //BindingContext = _viewModel = new ItemsViewModel();
            //Initialize the observable collection
            plants = new ObservableCollection<Plant>();

            //Go to async method to populate plants using database
            //SetPlants();

            //Make the Collection view set to plantscmd
            ItemsListView.ItemsSource = plants;

        }
        private async void SetPlants()
        {
            plants.Clear();
            //Query string
            string query = "SELECT unique_plant_id, plant_common_name, plant_scientific_name, light_level, moisture_level, active, plant_nick_name, plant_info FROM public.\"PlantList\" WHERE user_id = " + App.User.UserID + ";";

            //Reader that gets the data from the database after the written query has been run by the App's Database
            NpgsqlDataReader reader = await App.Database.Select(query);

            //While there are still results to read, read them. 
            while (await reader.ReadAsync())
            {
                //Based on the data read from the database,
                //Make a new plant with the information 
                Plant newPlant = new Plant
                {
                    Plant_ID = reader.GetInt32(0),
                    CommonName = reader.GetString(1),
                    ScientificName = reader.GetString(2),
                    Light = reader.GetString(3),
                    Moisture = reader.GetString(4),
                    Active = reader.GetBoolean(5),
                    UserDefinedName = reader.GetString(6)
                };

                //Add the new plant that was made to the Observable Collection of plants that
                //will be displayed
                if (newPlant.Active == true)
                {
                    plants.Add(newPlant);
                }
            }
        }
        protected override void OnAppearing()
        {
            //Displays the data onto page
            base.OnAppearing();

            SetPlants();
        }

        public async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Creating a variable for the plant
            Plant plant = e.CurrentSelection.FirstOrDefault() as Plant;
            if (plant != null)
            {
                await Navigation.PushAsync(new ItemDetailPage(plant));
            }

        }

        //Function that will navigate to another page once the button is pushed
        private async void OnButtonClick(object sender, EventArgs e)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Navigation.PushAsync(new PlantDatabasePage());
        }

        private async void OnArchiveClick(object sender, EventArgs e)
        {            
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Navigation.PushAsync(new ArchivePage());
        }

        //Function that will set the serach filter variable for comparrison
        private void RadioButtonClicked(object sender, EventArgs e)
        {
            RadioButton searchFilter = (RadioButton)sender;
            searchFilterSelected = searchFilter.Content.ToString();
        }


        //Function that will compare the serach filter variable and the user input
        public void ItemsPageSearchChanged(object sender, TextChangedEventArgs e)
        {
            //Gets the text to be compared
            string textValues = e.NewTextValue.ToLower();

            //Switch statement for different serach cases
            switch (searchFilterSelected)
            {
                case "Common Name":
                    //Checking the start of user input to see if the words match
                    var commonNameFilteredList = plants.Where(p => p.CommonName.ToLower().Contains(textValues));

                    ItemsListView.ItemsSource = commonNameFilteredList;
                    break;

               case "Scientifc Name":
                    //Checking the start of user input to see if the words match
                    var scientificNameFilteredList = plants.Where(p => p.ScientificName.ToLower().Contains(textValues));

                    ItemsListView.ItemsSource = scientificNameFilteredList;
                    break;

                default:
                    //Checking the start of user input to see if the words match
                    var defaultFilteredList = plants.Where(p => p.CommonName.ToLower().Contains(textValues));

                    ItemsListView.ItemsSource = defaultFilteredList;
                    break;
            }
        }
    }
}
