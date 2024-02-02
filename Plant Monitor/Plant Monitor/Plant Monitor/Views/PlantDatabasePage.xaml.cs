/***********
* Class: PlantDatabasePage
*
* Purpose:
*	The purpose of this class is to manage the plant database page for the app.
*
* Manager Functions:
*	PlantDatabasePage()
*		Basic constructor that also will query the database and populate the collection view with the plant data
*		
*
* Methods:
*	OnItemChanged(object sender, SelectionChangedEventArgs e)
*		When an item is selected from the collection view, go and display the data for that plant
*	
*	RadioButtonClicked(object sender, EventArgs e)
*		Registers a filter for the search feature
*	
*	PlantDatabaseSearchChanged(object sender, TextChangedEventArgs e)
*		When text is changed in the search bar, and based on the applied filter, search results appear. 
*
***********/
using System;
using Plant_Monitor.Models;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Npgsql;

namespace Plant_Monitor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlantDatabasePage : ContentPage
	{
		ObservableCollection<Plant> plants;
		private string searchFilterSelected { get; set; }
		private NpgsqlDataReader reader { get; set; }

		/* Purpose: Constructor PlantDatabasePage, 
		 * initialize and display the list of plants for the database to display on page
		 * Input: None
		 * Output: New instance of the Plant Database Page
		 */
		public PlantDatabasePage()
		{
			InitializeComponent();

			//Initialize the observable collection
			plants = new ObservableCollection<Plant>();

			//Go to async method to populate plants using database
			SetPlants();

			//Make the Collection view set to plantscmd
			PlantDatbaseCollectionView.ItemsSource = plants;
		}

		/* Purpose: Separate async method for accessing database and retrieving all of the 
		 *			data from the plant database. 
		 *			Iterates through the results and makes plant objects based on the 
		 *			data in the table. 
		 * Input: None
		 * Output: plants is populated with all of the necessary plant data
		 */
		private async void SetPlants()
		{
			/*Order of Results:
			0 - plant_id
			1 - common_name
			2 - scientific_name
			3 - light_level
			4 - moisture_level
			5 - plant_info
			 */

			//Query string
			string query = "SELECT plant_id, plant_common_name, plant_scientific_name, optimal_light_level, optimal_moisture_level, plant_info FROM public.\"PlantDatabase\" ORDER BY \"plant_common_name\" ASC;";
			
			try
			{
				//Reader that gets the data from the database after the written query has been run by the App's Database
				//Store the results 
				reader = await App.Database.Select(query);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: query  - PlantDatabasePage");
			}
			

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
					PlantInfo = reader.GetString(5)
				};

				//Add the new plant that was made to the Observable Collection of plants that
				//will be displayed
				plants.Add(newPlant);
			}
		}

		/* Purpose: Navigate to the page to display more details about the selected plant
		 * Input: object, SelectionChangedEventArgs
		 * Output: Has been navigated to the Plant Detail Page from the Database
		 */
		public async void OnItemChanged(object sender, SelectionChangedEventArgs e)
		{
			Plant plant = e.CurrentSelection.FirstOrDefault() as Plant;
			if (plant != null)
				await Navigation.PushAsync(new PlantDatabaseDetailPage(plant));
		}

		/* Purpose: Set the current filter for the search bar 
		 * Input: object, EventArgs
		 * Output: searchFilterSelected has the string value of whatever radio button has been selected
		 */
		private void RadioButtonClicked(object sender, EventArgs e)
		{
			RadioButton searchFilter = (RadioButton)sender;
			searchFilterSelected = searchFilter.Content.ToString();
		}

		/* Purpose: Display filtered search results based on the entered characters and the radio button selected 
		 * Input: object, TextChangedEventArgs
		 * Output: Filtered list based on search results. 
		 */
		public void PlantDatabaseSearchChanged(object sender, TextChangedEventArgs e)
		{
			string textValues = e.NewTextValue.ToLower();
			switch (searchFilterSelected)
			{
				case "Common Name":
					var commonNameFilteredList = plants.Where(p => p.CommonName.ToLower().Contains(textValues));

					PlantDatbaseCollectionView.ItemsSource = commonNameFilteredList;
					break;

				case "Scientific Name":
					var scientificNameFilteredList = plants.Where(p => p.ScientificName.ToLower().Contains(textValues));

					PlantDatbaseCollectionView.ItemsSource = scientificNameFilteredList;
					break;

				default:
					var defaultFilteredList = plants.Where(p => p.CommonName.ToLower().Contains(textValues));

					PlantDatbaseCollectionView.ItemsSource = defaultFilteredList;
					break;
			}
			
		}
	}
}