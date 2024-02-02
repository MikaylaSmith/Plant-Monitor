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
    public partial class ReportPage : ContentPage
    {
        private string plant_id;
        ObservableCollection<History> history;
        public ReportPage(string id)
        {
            InitializeComponent();
            plant_id = id;
            //BindingContext = _viewModel = new ItemsViewModel();
            //Initialize the observable collection
            history = new ObservableCollection<History>();


            //Make the Collection view set to plantscmd
            ReportView.ItemsSource = history;

        }
        private async void SetHistory()
        {
            history.Clear();
            //Query string
            string query = "SELECT row_id, unique_plant_id, light_level, moisture_level FROM public.\"PlantHistory\" ORDER BY row_id DESC ;" ;

            //Reader that gets the data from the database after the written query has been run by the App's Database
            NpgsqlDataReader reader = await App.Database.Select(query);

            //While there are still results to read, read them. 
            while (await reader.ReadAsync())
            {
                //Based on the data read from the database,
                //Make a new plant with the information 
                History newHistory = new History
                {
                    Row_id = reader.GetInt32(0),
                    Unique_id = reader.GetInt32(1),
                    Light = reader.GetString(2),
                    Moisture = reader.GetString(3)
                };
                if(newHistory.Unique_id.ToString() == plant_id)
                {
                    history.Add(newHistory);
                }
            }
        }
        protected override void OnAppearing()
        {
            //Displays the data onto page
            base.OnAppearing();

            SetHistory();
        }
    }
}
