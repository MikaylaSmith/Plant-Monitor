using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Plant_Monitor.Models;
using Xamarin.Forms;

namespace Plant_Monitor.ViewModels
{
    [QueryProperty(nameof(Common_Name), nameof(Common_Name))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private Plant plant1;


        public ItemDetailViewModel(Plant plant1)
        {
            this.plant1 = plant1;
        }


        //Creating the get and set functions for Common_Name
        public string Light
        {
            get
            {
                return Light;
            }
            set
            {
                Light = plant1.Light;
            }
        }

        //Creating the get and set functions for Moisture
        public string Moisture
        {
            get
            {
                return Moisture;
            }
            set
            {
                Moisture = plant1.Moisture;
            }
        }

        //Creating the get and set functions for Common_Name
        public string Common_Name
        {
            get
            {
                return Common_Name;
            }
            set
            {
                Common_Name = plant1.CommonName;
            }
        }

        //Creating the get and set functions for Scientific_Name
        public string Scientific_Name
        {
            get
            {
                return Scientific_Name;
            }
            set
            {
                Scientific_Name = plant1.ScientificName;
            }
        }
        private async void OnClick()
        {
            // This will pop the current page off the navigation stack
            //await Shell.Current.GoToAsync($"//{nameof(ReportPage)}");
        }
    }
}
