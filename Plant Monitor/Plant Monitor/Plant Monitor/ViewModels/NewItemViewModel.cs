using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Plant_Monitor.Models;
using Xamarin.Forms;

namespace Plant_Monitor.ViewModels
{
        public class NewItemViewModel : BaseViewModel
    {
         //Creating the variables
        private string commonname;
        private string nickname;

        //Constructor
        public NewItemViewModel()
        {
            //Creating the save command
            SaveCommand = new Command(OnSave, ValidateSave);
            //Creating the cancel command
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(CommonName);
        }

        //Creating the get and set for CommonName
        public string CommonName
        {
            get => commonname;
            set => SetProperty(ref commonname, value);
        }
        public string NickName
        {
            get => nickname;
            set => SetProperty(ref nickname, " ");
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        //Creating the OnCancel function
        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        //Creating the OnSave function
        private async void OnSave()
        {
            //String query = "INSERT INTO public.\"PlantDatabase\";(plant_id, plant_common_name, plant_scientific_name, optimal_light_level, optimal_moisture_level) VALUES('" + 9 + "','" + commonname + "', '" + scientificname + "', '" + LightLevel + "', '" + MoistureLevel + "')";
            //Setting all the variables for newItem to the values from the Plant
            try
            {
                App.Database.AddData(CommonName, NickName);

                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync("..");
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed to save item please enter a correct plant common name", "Ok");
            }

        }
    }
}
