/***********
* Class: PlantDatabaseDetailPage
*
* Purpose:
*	The purpose of this class is to display the plant data, specifically
*	plant details of a plant selected from the plant database.
*
* Manager Functions:
*	PlantDatabaseDetailPage()
*		No arg basic constructor, only initializes a blank page
*		
*	PlantDatabaseDetailPage(Plant plant)
*		Basic constructor, passed the plant that was selected to have
*		more details given. 
*		
*
* Methods:
*	None, all work is done solely in constructors
*
***********/
using Plant_Monitor.Models;
using System;
using Xamarin.Forms;

namespace Plant_Monitor.Views
{
	public partial class PlantDatabaseDetailPage : ContentPage
	{
		/* Purpose: Constructor PlantDatabaseDetailPage
		 * Input: None
		 * Output: New blank instance of the PlantDatabaseDetailsPage
		 *		   with no information on it at all. 
		 */
		public PlantDatabaseDetailPage()
		{
			InitializeComponent();
		}

		/* Purpose: Constructor PlantDatabaseDetailPage
		 * Input: A plant object that was selected in the PlantDatabasePage
		 * Output: New instance of the PlantDatabaseDetailsPage
		 *		   with all of the associated plant object information displayed.
		 */
		public PlantDatabaseDetailPage(Plant plant)
		{
			InitializeComponent();

			CommonName.Text = plant.CommonName;
			ScientificName.Text = plant.ScientificName;
			LightLevel.Text = plant.Light.ToString();
			MoistureLevel.Text = plant.Moisture.ToString();
			PlantInfo.Text = plant.PlantInfo;
		}

		public async void Add_Plant(object sender, EventArgs e)
		{
			App.Database.AddData(CommonName.Text, string.Empty);
			await Shell.Current.GoToAsync("..");
		}
	}
}