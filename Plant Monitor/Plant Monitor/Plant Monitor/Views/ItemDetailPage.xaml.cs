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

	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
		}
		public ItemDetailPage(Plant plant)
		{
			InitializeComponent();

			NickName.Text = plant.UserDefinedName;
			CommonName.Text = plant.CommonName;
			ScientificName.Text = plant.ScientificName;
			LightLevel.Text = plant.Light.ToString();
			MoistureLevel.Text = plant.Moisture.ToString();
			PlantInfo.Text = plant.PlantInfo;
			PlantID.Text = plant.Plant_ID.ToString();
		}
		private async void OnRemoved(object sender, EventArgs e)
		{
			try
			{
				App.Database.UpdateActive(CommonName.Text);
				await Shell.Current.GoToAsync("..");
			}
			catch
            {
				await DisplayAlert("Error","Failed to delete item", "Ok");
			}
		}
		private async void OnButtonClick(object sender, EventArgs e)
		{
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			await Navigation.PushAsync(new ReportPage(PlantID.Text));
		}
	}
}
